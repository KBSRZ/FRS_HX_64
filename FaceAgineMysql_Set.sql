CREATE DATABASE `frs_database_set`;

USE `frs_database_set`;

create table `frs_database_set`.`dataset`
(
	`id` int(11) AUTO_INCREMENT,	
	`datasetname` nvarchar(50) NOT NULL,
	`type` int(11) NULL,
	`user` nvarchar(20) NULL,
	`password` nvarchar(20) NULL,
	`ip` nvarchar(20) NULL,
	`port` nvarchar(20) NULL,
	`remark` nvarchar(50) NULL,
	PRIMARY KEY (`id`),
	UNIQUE KEY `id` (`id`)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;

create table `frs_database_set`.`device`
(
	`id` int(11) AUTO_INCREMENT,	
	`name` nvarchar(50) NOT NULL,
	`address` nvarchar(50) NOT NULL,
	`departmentmentid` nvarchar(50) NULL,
	`longitude` double(5,2) NULL,
	`latitude` double(5,2) NULL,
	`locationtype` int(11) NULL,
	`remark` nvarchar(50) NULL,
	PRIMARY KEY (`id`),
	UNIQUE KEY `id` (`id`)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;

create table `frs_database_set`.`surveillancetask`
(
	`id` int(11) AUTO_INCREMENT,
	`name` nvarchar(50) NOT NULL,
	`databaseid` int(11) NOT NULL,
	`deviceid` int(11) NOT NULL,
	`type` int(11) NULL,
	`remark` nvarchar(50) NULL,
	PRIMARY KEY (`id`),
	UNIQUE KEY `id` (`id`)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;


INSERT INTO `frs_database_set`.`dataset` (`datasetname`) VALUES ('frsdb');

