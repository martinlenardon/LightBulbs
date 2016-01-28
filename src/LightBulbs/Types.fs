namespace LightBulbs

[<Measure>] type W
[<Measure>] type lm
[<Measure>] type h
[<Measure>] type day
[<Measure>] type month
[<Measure>] type year
[<Measure>] type USD
[<Measure>] type kW
[<Measure>] type kWh = kW h

[<AutoOpen>]
module Types =
    let WattsPerKiloWatt : float<W/kW> = 1000.0<W/kW>
    let HoursPerYear : float<h/year> = 365.0<day/year>*24.0<h/day>
    let HoursPerMonth : float<h/month> = (365.0<day/year>/12.0<month/year>)*24.0<h/day>
    let MonthsPerYear : float<month/year> = 12.0<month/year>
    let DaysPerMonth : float<day/month> = 365.0<day/year>/12.0<month/year>

    type LightBulb = {
        Watts : float<W>
        Lumens : float<lm>
        Lifetime : float<h>
        Cost : float<USD>
    }

    type Scenario = {
        CostInflation : float<1/year>
        DiscountRate : float<1/year>
        TimeHorizon : float<year>
        HoursPerDay : float<h/day>
        PowerCost: float<USD/kWh>
        PowerInflation : float<1/year>
    }