use mysql;
Delete FROM user Where User='frba' and Host='localhost';
flush privileges;
drop database frsdb; 
drop user 'frba'@'%' ;
drop user 'frba'@'localhost'; 