DROP TABLE IF EXISTS `eventlogs`;

CREATE TABLE `eventlogs`  (
  `EventId` bigint(20) NOT NULL COMMENT '',
  `MessageId` varchar(50)  NOT NULL COMMENT '',
	`TraceId` varchar(50) NULL DEFAULT NULL COMMENT '',
	`EventTypeName` varchar(500) NULL DEFAULT NULL COMMENT '事件类型',
  `State` int(11) NOT NULL COMMENT '状态 0:未发送 1:已发送 2:发送失败',
  `TimesSent` int(11) NOT NULL COMMENT '发送次数',
	`CreationTime` datetime NOT NULL COMMENT '创建时间',
	`Content` varchar(8000)  NULL DEFAULT NULL COMMENT '内容',
  `Partition` int(11) NOT NULL COMMENT '分区数量',
  PRIMARY KEY (`EventId`) USING BTREE
) ENGINE=InnoDB DEFAULT COMMENT '事件信息表';