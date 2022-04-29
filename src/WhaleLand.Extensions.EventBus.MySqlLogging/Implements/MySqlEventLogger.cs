﻿using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using WhaleLand.Extensions.EventBus.Abstractions;
using WhaleLand.Extensions.EventBus.Models;
using WhaleLand.Extensions.UidGenerator;

namespace WhaleLand.Extensions.EventBus.MySqlLogging
{
    public class MySqlEventLogger : IEventLogger
    {
        readonly IDbConnectionFactory _dbConnection;
        readonly IUniqueIdGenerator _uniqueIdGenerator;
        readonly MySqlConfiguration _mySqlConfiguration;

        public MySqlEventLogger(
            IUniqueIdGenerator uniqueIdGenerator,
            IDbConnectionFactory dbConnection,
            MySqlConfiguration mySqlConfiguration)
        {

            this._mySqlConfiguration = mySqlConfiguration;
            this._uniqueIdGenerator = uniqueIdGenerator;
            this._dbConnection = dbConnection;
        }



        /// <summary>
        /// 保存事件
        /// </summary>
        /// <param name="LogEntrys"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public async Task<List<EventLogEntry>> SaveEventAsync(List<EventLogEntry> LogEntrys, IDbTransaction transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction", $"A {typeof(DbTransaction).FullName} is required as a pre-requisite to save the event.");
            }
            var sqlParamtersList = new List<DynamicParameters>();
            foreach (var eventLogEntry in LogEntrys)
            {
                if (eventLogEntry.EventId <= 0)
                {
                    eventLogEntry.EventId = _uniqueIdGenerator.NewId();
                }

                var sqlParamters = new DynamicParameters();
                sqlParamters.Add("EventId", eventLogEntry.EventId, System.Data.DbType.Int64, System.Data.ParameterDirection.Input, 6);
                sqlParamters.Add("MessageId", eventLogEntry.MessageId, System.Data.DbType.StringFixedLength, System.Data.ParameterDirection.Input, 50);
                sqlParamters.Add("TraceId", eventLogEntry.TraceId, System.Data.DbType.StringFixedLength, System.Data.ParameterDirection.Input, 50);
                sqlParamters.Add("EventTypeName", eventLogEntry.EventTypeName, System.Data.DbType.StringFixedLength, System.Data.ParameterDirection.Input, 500);
                sqlParamters.Add("State", eventLogEntry.State, System.Data.DbType.Int32, System.Data.ParameterDirection.Input, 4);
                sqlParamters.Add("TimesSent", 0, System.Data.DbType.Int32, System.Data.ParameterDirection.Input, 4);
                sqlParamters.Add("CreationTime", DateTime.UtcNow, System.Data.DbType.Date, System.Data.ParameterDirection.Input, 4);
                sqlParamters.Add("Content", eventLogEntry.Content, System.Data.DbType.StringFixedLength, System.Data.ParameterDirection.Input);
                sqlParamtersList.Add(sqlParamters);
            }

            await transaction.Connection.ExecuteAsync($"insert into {_mySqlConfiguration.TablePrefix}EventLogs(EventId,MessageId,TraceId,EventTypeName,State,TimesSent,CreationTime,Content) values(@EventId,@MessageId,@TraceId,@EventTypeName,@State,@TimesSent,@CreationTime,@Content)",
            sqlParamtersList,
            transaction: transaction
            );

            return LogEntrys;
        }

        /// <summary>
        /// 事件发布成功
        /// </summary>
        /// <param name="events"></param>
        /// <returns></returns>
        public async Task MarkEventAsPublishedAsync(List<long> events, CancellationToken cancellationToken)
        {
            if (events != null)
            {
                var sqlParamtersList = new List<DynamicParameters>();
                foreach (var eventId in events)
                {
                    var sqlParamters = new DynamicParameters();
                    sqlParamters.Add("EventId", eventId, System.Data.DbType.Int64, System.Data.ParameterDirection.Input, 6);
                    sqlParamtersList.Add(sqlParamters);
                }

                using (var db = _dbConnection.GetDbConnection())
                {
                    if (db.State != System.Data.ConnectionState.Open)
                    {
                        await db.OpenAsync(cancellationToken);
                    }
                    using (var tran = db.BeginTransaction())
                    {
                        await db.ExecuteAsync($"delete from {_mySqlConfiguration.TablePrefix}EventLogs where EventId=@EventId", sqlParamtersList, transaction: tran);
                        tran.Commit();
                    }
                }

            }
        }

        /// <summary>
        /// 事件发布失败
        /// </summary>
        /// <param name="events"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task MarkEventAsPublishedFailedAsync(List<long> events, CancellationToken cancellationToken)
        {
            if (events != null)
            {
                var sqlParamtersList = new List<DynamicParameters>();
                foreach (var eventId in events)
                {
                    var sqlParamters = new DynamicParameters();
                    sqlParamters.Add("EventId", eventId, System.Data.DbType.Int64, System.Data.ParameterDirection.Input, 6);
                    sqlParamtersList.Add(sqlParamters);
                }

                using (var db = _dbConnection.GetDbConnection())
                {
                    if (db.State != System.Data.ConnectionState.Open)
                    {
                        await db.OpenAsync(cancellationToken);
                    }
                    using (var tran = db.BeginTransaction())
                    {
                        await db.ExecuteAsync($"update {_mySqlConfiguration.TablePrefix}EventLogs set TimesSent=TimesSent+1,State=2 where EventId=@EventId", sqlParamtersList, transaction: tran);

                        tran.Commit();
                    }
                }
            }

            return;
        }


        /// <summary>
        /// 发送发送失败或没有发送的消息
        /// </summary>
        /// <param name="Take"></param>
        /// <returns></returns>
        public List<EventLogEntry> GetUnPublishedEventList(int Take)
        {
            using (var db = _dbConnection.GetDbConnection())
            {
                return db.Query<EventLogEntry>($"select EventId,MessageId,TraceId,EventTypeName,State,TimesSent,CreationTime,Content from {_mySqlConfiguration.TablePrefix}EventLogs where (State=0 or State=2) and TimesSent<=3 order by EventId asc limit {Take}").AsList();
            }
        }
    }
}
