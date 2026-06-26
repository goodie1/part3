--creating a database called progdemo--
create database progdemo;

--use the progdemo database--
use [progdemo];

--Creating a table called demo_tasks
--colums are task_id,task_name,task_description,task_duedate,task_status
--List of the columes 
--task_id Int Primary key, and auto-incremment
--task_name varchar(255) 
--task_description varchar (255) 
--task_duedate varchar()
--task_status varchar() 
create table demo_tasks(
task_id Int Primary key identity(1,1),
task_name varchar(500),
task_description varchar(500),
task_duedate varchar(20),
task_status varchar(20)
);
--selected all colums from table demo_tasks
select * from demo_tasks;




