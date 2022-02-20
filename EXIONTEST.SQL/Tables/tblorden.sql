CREATE TABLE [dbo].[tblorden]
(
	oid					int not null identity(1, 1) constraint pk_tblorden primary key
	, cid				int not null
	, toid				int	not null
	, eid				int	not null
	, acciones			int not null
	, fecharegistro		datetime not null default getdate()
	, constraint fk_tblcuenta foreign key (cid) references tblcuenta(cid)
	, constraint fk_tbltipooperacion foreign key (toid) references tbltipooperacion(toid)
	, constraint fk_tblemisor foreign key(eid) references tblemisor(eid)
)