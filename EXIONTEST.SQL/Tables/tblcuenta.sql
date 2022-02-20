CREATE TABLE [dbo].[tblcuenta]
(
	cid					int not null identity(1, 1) constraint pk_tblcuenta primary key
	, saldo				decimal(20, 2)
)