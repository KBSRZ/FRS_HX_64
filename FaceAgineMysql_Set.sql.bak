CREATE DATABASE `frs_database_set`;

USE `frs_database_set`;

create table `frs_database_set`.`dataset`
(
	`id` int(11) AUTO_INCREMENT,	
	`datasetname` nvarchar(50) NOT NULL,--数据库名称，暂时只有它有用
	`type` int(11) NULL,--类型
	`user` nvarchar(20) NULL,--用户名
	`password` nvarchar(20) NULL,--密码
	`ip` nvarchar(20) NULL,--ip地址
	`port` nvarchar(20) NULL,--端口
	`remark` nvarchar(50) NULL,--备注
	PRIMARY KEY (`id`),
	UNIQUE KEY `id` (`id`)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;

create table `frs_database_set`.`device`
(
	`id` int(11) AUTO_INCREMENT,	
	`name` nvarchar(50) NOT NULL,  --数据库名称，暂时只有它有用
	`address` nvarchar(50) NOT NULL,--视频流地址
	`departmentmentid` nvarchar(50) NULL,--公安部门ID
	`longitude` double(5,2) NULL,--经度
	`latitude` double(5,2) NULL,--纬度
	`locationtype` int(11) NULL,--区域类型，酒吧之类的
	`remark` nvarchar(50) NULL,--备注
	PRIMARY KEY (`id`),
	UNIQUE KEY `id` (`id`)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;

create table `frs_database_set`.`surveillancetask`
(
	`id` int(11) AUTO_INCREMENT,
	`name` nvarchar(50) NOT NULL,--名称
	`databaseid` int(11) NOT NULL,
	`deviceid` int(11) NOT NULL,
	`type` int(11) NULL,
	`remark` nvarchar(50) NULL,--备注
	PRIMARY KEY (`id`),
	UNIQUE KEY `id` (`id`)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;


INSERT INTO `frs_database_set`.`dataset` (`datasetname`) VALUES ('frsdb');

