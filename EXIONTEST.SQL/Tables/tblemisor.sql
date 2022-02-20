CREATE TABLE [dbo].[tblemisor]
(
	eid					int not null identity(1, 1) constraint pk_tblemisor primary key
	, nombre			varchar(50) not null
	, stock				int not null
	, precio			decimal(20, 2) not null
)