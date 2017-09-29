CREATE DATABASE `frsdb`;

USE `frsdb`;

create table `frsdb`.`user`
(
	`id` int(11) AUTO_INCREMENT,
	`people_id` varchar(50) NULL,
	`name` nvarchar(50) NULL,
	`gender` char(1) NULL,
	`card_id` varchar(50) NULL,
	`image_id` varchar(60) NULL,
	`face_image_path` varchar(200) NOT NULL,
	`feature_data` LongBlob NOT NULL,
	`type` char NULL,
	`create_time` datetime NOT NULL,
	`modified_time` datetime NOT NULL,
	`quality_score` float,
	PRIMARY KEY (`id`),
	UNIQUE KEY `id` (`id`)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;

create table `frsdb`.`hitrecord`
(
	`id` int(11) AUTO_INCREMENT,
	`face_query_image_path` varchar(200) NOT NULL,
	`threshold` float NOT NULL,
	`occur_time` datetime NOT NULL,
	PRIMARY KEY (`id`),
	UNIQUE KEY `id` (`id`)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;

create table `frsdb`.`hitrecord_detail`
(
	`id` int(11) AUTO_INCREMENT,
	`hit_record_id` int(11) NOT NULL,
	`user_id` int(11) NOT NULL, 
	`rank` int(11) NOT NULL,
	`score` float NOT NULL,
	PRIMARY KEY (`id`),
	UNIQUE KEY `id` (`id`)
)ENGINE=InnoDB DEFAULT CHARSET=utf8;

create view `frsdb`.`hitalert` as 
select 
hit.id,
hit.face_query_image_path,
hit.threshold,
hit.occur_time,
detail.id as detail_id,
detail.rank,
detail.score,
usr.id as user_id,
usr.name as user_name,
usr.gender as user_gender,
usr.card_id as user_card_id,
usr.people_id as user_people_id,
usr.image_id as user_image_id,
usr.face_image_path as user_face_image_path,
usr.type as user_type,
usr.create_time as user_create_time,
usr.modified_time as user_modified_time,
usr.quality_score as user_quality_score
FROM 
(`frsdb`.`hitrecord_detail` as detail
left join `frsdb`.`hitrecord` as hit on detail.hit_record_id=hit.id) 
left join `frsdb`.`user` as usr on detail.user_id = usr.id;