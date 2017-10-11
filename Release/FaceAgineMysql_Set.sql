CREATE DATABASE `frs_database_set`;

USE `frs_database_set`;

create table `frs_database_set`.`table`
(
	`id` int(11) AUTO_INCREMENT,	
	`name` nvarchar(50) NOT NULL,
	PRIMARY KEY (`id`),
	UNIQUE KEY `id` (`id`)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;

create table `frs_database_set`.`device`
(
	`id` int(11) AUTO_INCREMENT,	
	`name` nvarchar(50) NOT NULL,
	`ip` nvarchar(50) NOT NULL,
	`port` nvarchar(50) NOT NULL,
	`user` nvarchar(50) NOT NULL,
	`password` nvarchar(50) NOT NULL,
	PRIMARY KEY (`id`),
	UNIQUE KEY `id` (`id`)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;

INSERT INTO `frs_database_set`.`table` (`name`) VALUES ('frsdb');

