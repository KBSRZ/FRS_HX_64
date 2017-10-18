CREATE DATABASE `frs_database_set`;

USE `frs_database_set`;

create table `frs_database_set`.`frs_database`
(
	`id` int(11) AUTO_INCREMENT,	
	`name` nvarchar(50) NOT NULL,
	`type` nvarchar(8) NULL,
	`user` nvarchar(20) NULL,
	`password` nvarchar(20) NULL,
	`address` nvarchar(20) NULL,
	`info` nvarchar(50) NULL,
	PRIMARY KEY (`id`),
	UNIQUE KEY `id` (`id`)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;

create table `frs_database_set`.`device`
(
	`id` int(11) AUTO_INCREMENT,	
	`name` nvarchar(50) NOT NULL,
	`ip` nvarchar(20) NOT NULL,
	`port` nvarchar(20) NOT NULL,
	`user` nvarchar(20) NOT NULL,
	`password` nvarchar(50) NOT NULL,
	`info` nvarchar(20) NULL,
	PRIMARY KEY (`id`),
	UNIQUE KEY `id` (`id`)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;

create table `frs_database_set`.`task`
(
	`id` int(11) AUTO_INCREMENT,	
	`databaseid` int(11) NOT NULL,
	`deviceid` int(11) NOT NULL,
	PRIMARY KEY (`id`),
	UNIQUE KEY `id` (`id`)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;


INSERT INTO `frs_database_set`.`frs_database` (`name`) VALUES ('frsdb');

