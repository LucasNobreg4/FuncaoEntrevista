-- =============================================
-- STORED PROCEDURES PARA BENEFICIÁRIOS
-- Execute este script no banco de dados
-- =============================================

-- 1. Criar tabela BENEFICIARIOS
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'BENEFICIARIOS')
BEGIN
    CREATE TABLE BENEFICIARIOS (
        Id BIGINT IDENTITY(1,1) PRIMARY KEY,
        CPF VARCHAR(11) NOT NULL,
        Nome VARCHAR(100) NOT NULL,
        IdCliente BIGINT NOT NULL,
        CONSTRAINT FK_BENEFICIARIOS_CLIENTES FOREIGN KEY (IdCliente) REFERENCES CLIENTES(Id)
    );
    
    CREATE INDEX IX_BENEFICIARIOS_IdCliente ON BENEFICIARIOS(IdCliente);
    CREATE INDEX IX_BENEFICIARIOS_CPF ON BENEFICIARIOS(CPF);
    
    PRINT 'Tabela BENEFICIARIOS criada com sucesso!';
END
GO

-- 2. SP Incluir Beneficiário
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'FI_SP_IncBeneficiario')
    DROP PROCEDURE FI_SP_IncBeneficiario
GO

CREATE PROCEDURE FI_SP_IncBeneficiario
    @CPF VARCHAR(11),
    @Nome VARCHAR(100),
    @IdCliente BIGINT
AS
BEGIN
    INSERT INTO BENEFICIARIOS (CPF, Nome, IdCliente)
    VALUES (@CPF, @Nome, @IdCliente);
    
    SELECT SCOPE_IDENTITY() AS Id;
END
GO

-- 3. SP Alterar Beneficiário
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'FI_SP_AltBeneficiario')
    DROP PROCEDURE FI_SP_AltBeneficiario
GO

CREATE PROCEDURE FI_SP_AltBeneficiario
    @Id BIGINT,
    @CPF VARCHAR(11),
    @Nome VARCHAR(100),
    @IdCliente BIGINT
AS
BEGIN
    UPDATE BENEFICIARIOS
    SET CPF = @CPF,
        Nome = @Nome,
        IdCliente = @IdCliente
    WHERE Id = @Id;
END
GO

-- 4. SP Consultar Beneficiário
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'FI_SP_ConsBeneficiario')
    DROP PROCEDURE FI_SP_ConsBeneficiario
GO

CREATE PROCEDURE FI_SP_ConsBeneficiario
    @Id BIGINT
AS
BEGIN
    SELECT Id, CPF, Nome, IdCliente
    FROM BENEFICIARIOS
    WHERE Id = @Id;
END
GO

-- 5. SP Listar Beneficiários por Cliente
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'FI_SP_ListBeneficiariosPorCliente')
    DROP PROCEDURE FI_SP_ListBeneficiariosPorCliente
GO

CREATE PROCEDURE FI_SP_ListBeneficiariosPorCliente
    @IdCliente BIGINT
AS
BEGIN
    SELECT Id, CPF, Nome, IdCliente
    FROM BENEFICIARIOS
    WHERE IdCliente = @IdCliente
    ORDER BY Nome;
END
GO

-- 6. SP Excluir Beneficiário
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'FI_SP_DelBeneficiario')
    DROP PROCEDURE FI_SP_DelBeneficiario
GO

CREATE PROCEDURE FI_SP_DelBeneficiario
    @Id BIGINT
AS
BEGIN
    DELETE FROM BENEFICIARIOS
    WHERE Id = @Id;
END
GO

-- 7. SP Verificar CPF Duplicado
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'FI_SP_VerificaBeneficiarioCPF')
    DROP PROCEDURE FI_SP_VerificaBeneficiarioCPF
GO

CREATE PROCEDURE FI_SP_VerificaBeneficiarioCPF
    @CPF VARCHAR(11),
    @IdCliente BIGINT,
    @IdBeneficiario BIGINT = 0
AS
BEGIN
    SELECT Id
    FROM BENEFICIARIOS
    WHERE CPF = @CPF
      AND IdCliente = @IdCliente
      AND Id <> @IdBeneficiario;
END
GO

-- 8. SP Consultar Cliente por CPF de Beneficiário
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'FI_SP_ConsClientePorCPFBeneficiario')
    DROP PROCEDURE FI_SP_ConsClientePorCPFBeneficiario
GO

CREATE PROCEDURE FI_SP_ConsClientePorCPFBeneficiario
    @CPF VARCHAR(11)
AS
BEGIN
    SELECT TOP 1 IdCliente
    FROM BENEFICIARIOS
    WHERE CPF = @CPF;
END
GO

DROP PROCEDURE IF EXISTS FI_SP_AltBenef;

PRINT 'Todas as Stored Procedures foram criadas com sucesso!';
