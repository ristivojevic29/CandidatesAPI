CREATE FUNCTION ExchangeRate(@currency_from NVARCHAR(10), @currency_to NVARCHAR(10), @exchange_rate_date DATE)
RETURNS DECIMAL (18,4)
AS
BEGIN
	DECLARE @rate DECIMAL (18,4)
	IF @currency_from = @currency_to
		RETURN 1
	SELECT @rate = exchange_rate
	FROM dbo.exchange_rates
	WHERE currency_from = @currency_from
	AND currency_to = @currency_to
	AND exchange_rate_date = @exchange_rate_date
	RETURN ISNULL (@rate, 0)
END
GO

DECLARE @dateFrom DATE = '2024-01-01'
DECLARE @dateTo  DATE = '2025-05-24'
DECLARE @currency nvarchar(10) = 'RSD'
DECLARE @exchange_rate_date DATE = '2024-10-09'

SELECT dbo.employees.employee_id as EmployeeID, dbo.employees.name as FullName, 
CASE 
	WHEN dbo.employees.active_to IS NULL THEN 'active'
	ELSE 'not active'
END AS Active,
COUNT(contract_id) as NumOfContracts,
CAST(SUM(contract_value * dbo.ExchangeRate(currency_code, @currency, @exchange_rate_date)) AS DECIMAL (18,2)) as ValueOfContracts,
@currency AS Currency,
AVG(duration) as AvgDurationOfContracts,
CAST(MAX(contract_value * dbo.ExchangeRate(currency_code, @currency, @exchange_rate_date)) AS DECIMAL (18,2)) as BiggestContract,
(SELECT TOP 1 activation_date FROM dbo.contracts WHERE dbo.contracts.employee_id = dbo.employees.employee_id ORDER BY activation_date DESC) as LastActivatedContract
FROM dbo.employees
LEFT JOIN dbo.contracts ON dbo.employees.employee_id = dbo.contracts.employee_id
AND dbo.contracts.activation_date >= @dateFrom AND dbo.contracts.activation_date <= @dateTo
GROUP BY dbo.employees.employee_id, dbo.employees.name, dbo.employees.active_to
ORDER BY NumOfContracts DESC