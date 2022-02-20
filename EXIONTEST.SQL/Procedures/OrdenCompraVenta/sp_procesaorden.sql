CREATE PROCEDURE [dbo].[sp_procesaorden]
(
	@Request		nvarchar(max)
)
as
begin
	declare @counter int = 1
			, @maxid int
			, @json nvarchar(max)
			, @strerror nvarchar(max)

	declare @tbl table(id int identity(1, 1), json nvarchar(max))
	declare @result table(id int identity(1, 1), cid int, eid int, acciones int, error nvarchar(max))

	--begin tran
	begin try
		insert into @tbl
			select value from openjson(@Request)

		select @maxid = max(id) from @tbl
		while @counter <= @maxid
			begin
				select @json = json from @tbl where id = @counter

				exec spi_orden @json, @strerror output

				insert into @result
					select	orden.cid, orden.eid, orden.acciones, @strerror
					from openjson(@json)
					with
						(
							cid					int 'strict $.cid'
							, eid				int '$.eid'
							, acciones			int '$.acciones'
						) as orden

				set @counter += 1;
			end

		select	[Response] =
			(
				select	[cash]				= current_balance.saldo
						, [issuer_name]		= issuers.nombre
						, [total_shares]	= issuers.acciones
						, [share_price]		= issuers.precio
						, [business_errors] = issuers.error
				from tblcuenta		current_balance
				inner join @result	r
					on current_balance.cid = r.cid
				right outer join
					(
						select	i.nombre
								, ri.acciones
								, i.precio
								, ri.id
								, [error]	= isnull(ri.error, '')
						from @result ri
						inner join tblemisor i
							on ri.eid = i.eid
					)				issuers
					on issuers.id = r.id
				group by
					r.id
					, r.cid
					, current_balance.saldo
					, r.eid
					, issuers.acciones
					, issuers.nombre
					, issuers.precio
					, issuers.error
				for json auto, without_array_wrapper
			)
		
		--commit;
	end try
	begin catch
		--rollback;
		declare @emesage varchar(800) = 'SP_Name: ' + error_procedure() + '|Line: ' + cast(error_line() as varchar(5)) + '|Error: ' + error_message();

		select	[Response] = '{"errors":["' + @emesage + '"]}'
	end catch
end