CREATE PROCEDURE [dbo].[spi_orden]
(
	@Request		nvarchar(max)
	, @Error		nvarchar(max) output
)
as
begin
	declare @saldo decimal(20, 2)
			, @nsaldo decimal(20, 2)
			, @stock int
			, @nstock int
			, @precio decimal(20, 2)
			, @acciones int
			, @aacciones int
			, @nacciones int
			, @currenttime time = cast(getdate() as time)
			, @tipooperacion int

	declare @tblerror table(error nvarchar(100))
	declare @tbl table(id int identity(1, 1), cid int, toid int, eid int, acciones int, fecharegistro datetime)

	--begin transaction
	begin try
		/* Se parsea la cadena json a datos dentro de una tabla */
		insert into @tbl
			select	orden.*
			from openjson(@Request)
			with
				(
					cid					int 'strict $.cid'
					, toid				int '$.toid'
					, eid				int '$.eid'
					, acciones			int '$.acciones'
					, fecharegistro		datetime '$.fecharegistro'
				) as orden

		/* Se obtienen los datos a validar */
		select	@saldo = c.saldo
				, @nsaldo = case when t.toid = 1 then c.saldo - (e.precio * t.acciones) else c.saldo + (e.precio * t.acciones) end
				, @stock = e.stock
				, @nstock = case when t.toid = 1 then e.stock - t.acciones else e.stock + t.acciones end
				, @precio = e.precio * t.acciones
				, @acciones = t.acciones
				, @aacciones = isnull(o.acciones, 0)
				, @nacciones = case when t.toid = 1 then @aacciones + t.acciones else @aacciones - t.acciones end
				, @tipooperacion = t.toid
		from @tbl				t
		inner join tblcuenta	c
			on c.cid = t.cid
		inner join tblemisor	e
			on e.eid = t.eid
		left join tblorden		o
			on o.cid = t.cid
			and o.eid = t.eid

		/* Validacions por regla de negocio */
		if @tipooperacion = 1 and (@precio > isnull(@saldo, 0))
			begin
				insert into @tblerror values('Saldo insuficiente para realizar la compra.')
			end

		if @tipooperacion = 2 and @acciones > @aacciones
			begin
				insert into @tblerror values('El stock de la cuenta es insuficiente para realizar la venta.')
			end

		if @acciones > @stock
			begin
				insert into @tblerror values('El stock del emisor es insuficiente para realizar la compra.')
			end

		if @currenttime <= '06:00:00' or @currenttime >= '15:00:00'
			begin
				insert into @tblerror values('Mercado cerrado.')
			end
		
		if @tipooperacion not in (1, 2)
			begin
				insert into @tblerror values('Operación no válida.')
			end

		if not exists(select * from @tblerror)
			begin
				/* Actualiza los datos de la orden o la inserta en caso de no existir */
				merge tblorden			o
				using @tbl				t
					on o.cid = t.cid
					and o.eid = t.eid
				when matched then update
				set 
					o.acciones	= @nacciones
					, o.toid	= @tipooperacion
				when not matched then insert
				(
					cid
					, toid
					, eid
					, acciones
					, fecharegistro
				)
				values
				(
					t.cid
					, t.toid
					, t.eid
					, t.acciones
					, t.fecharegistro
				)
				output 
					inserted.cid
					, inserted.toid
					, inserted.eid
					, inserted.acciones
					, inserted.fecharegistro
				into @tbl;

				/* Actualiza el saldo de la cuenta */
				update	c
					set c.saldo = @nsaldo
				from tblcuenta			c
				inner join @tbl			t
					on c.cid = t.cid

				/* Actualiza el stock de acciones del emisor */
				update	e
					set e.stock = @nstock
				from tblemisor			e
				inner join @tbl			t
					on e.eid = t.eid
			end

		if @@trancount > 0
			begin
                commit;
			end

		select	@Error = N'["' + string_agg(error, '","') + '"]'
		--select	@Error = string_agg(error, '|')
		from @tblerror

		--select	c.saldo
		--		, e.nombre
		--		, e.precio
		--from @tbl					t
		--inner join tblcuenta		c
		--	on t.cid = c.cid
		--inner join tblemisor		e
		--	on t.eid = e.eid
		--for json auto
	end try
	begin catch
		if @@trancount > 0
			begin
                rollback;
			end

		declare @emesage varchar(800) = 'SP_Name: ' + error_procedure() + '|Line: ' + cast(error_line() as varchar(5)) + '|Error: ' + error_message();

		set @Error = '["' + @emesage + '"]'
	end catch
end