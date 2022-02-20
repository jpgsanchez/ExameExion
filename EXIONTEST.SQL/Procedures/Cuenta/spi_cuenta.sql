CREATE PROCEDURE [dbo].[spi_cuenta]
(
	@Request		nvarchar(max)
)
as
begin
	declare @saldo decimal(20, 2)
	declare @tbl as table(cid int, saldo decimal(20, 2))

	begin try
		select @saldo = json_value(@Request, '$.saldo')

		insert into
			tblcuenta
		output
			inserted.cid
			, inserted.saldo
		into @tbl
		values
			(
				@saldo
			)

		select	[Response] =
			(
				select	[id]		= cid
						, [cash]	= saldo
				from @tbl
				for json path, without_array_wrapper
			)
	end try
	begin catch
		declare @emesage varchar(800) = 'SP_Name: ' + error_procedure() + '|Line: ' + cast(error_line() as varchar(5)) + '|Error: ' + error_message();

		select	[Response] = '{"errors":["' + @emesage + '"]}'
	end catch
end