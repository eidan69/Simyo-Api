Feature: SimyoTools
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@mytag
Scenario Outline: Comprobando el casting de precio por string
	Given Queremos llamar a un método de SimyoTools
	When Cuando llamamos a getPrice con <Cantidad> como string
	Then El resultado obtenido debe de ser el <Salida>
	Scenarios:
	| Cantidad | Salida |
	| 333      | 333 €   |
	| 3.3      | 3.3 €   |
	| 2,1      | 2.1 €   |
	| 0.1      | 0.1 €   |
