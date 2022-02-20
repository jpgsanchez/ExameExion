CREATE TABLE [dbo].[tbltipooperacion]
(
	toid				int not null identity(1, 1) constraint pk_tbltipooperacion primary key
	, descripcion		varchar(50) not null
)