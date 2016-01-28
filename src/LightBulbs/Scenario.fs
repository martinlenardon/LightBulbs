namespace LightBulbs

module Scenario =
    let SimReplacementCost (s: Scenario) (bulb: LightBulb) :float<USD>=
        let times = LightBulb.SimBulbsNeeded bulb (s.TimeHorizon*HoursPerYear)
        let r = Discounting.YearlyToHourly s.DiscountRate
        let i = Discounting.YearlyToHourly s.CostInflation
        LightBulb.ReplacementCost bulb.Cost i r times

    let SimReplacementCosts (s: Scenario) (bulb: LightBulb) (n: int) :float<USD> list=
        List.map (fun _ -> SimReplacementCost s bulb) [1..n]

    let TheoreticalReplacementCost (s: Scenario) (bulb: LightBulb) :float<USD>=
        let t = s.TimeHorizon*HoursPerYear
        let alpha = (Discounting.YearlyToHourly s.DiscountRate) - (Discounting.YearlyToHourly s.CostInflation)
        let c = bulb.Cost
        let lambda = LightBulb.Rate bulb
        let coef = (c*lambda)/alpha
        let body = (1.0-(exp (-1.0*alpha*t)))
        coef*body+c

    let UsageCost (s: Scenario) (bulb: LightBulb) :float<USD> =
        let nFullMonths = (int (s.TimeHorizon * MonthsPerYear))
        let partialMonth = float (s.TimeHorizon*MonthsPerYear) - (float nFullMonths)
        let fullMonthUsage :float<kWh/month>= DaysPerMonth*s.HoursPerDay*(bulb.Watts/WattsPerKiloWatt)
        let fullMonths =
            [1..nFullMonths]
            |> List.map (fun x -> 1.0<month>*(float x))
        let months = List.append fullMonths [1.0<month>*float(nFullMonths+1)]
        let usage = 
            let full = List.map (fun _ -> fullMonthUsage) fullMonths
            (partialMonth*fullMonthUsage)::full
   
        let i = Discounting.YearlyToMonthly s.PowerInflation
        let r = Discounting.YearlyToMonthly s.DiscountRate
        List.zip months usage
        |> List.map (fun (t, u) -> (t, s.PowerCost*exp(i*t)*u*1.0<month>))
        |> Discounting.Discount (r)
    
    let TotalCostPerLumen (s: Scenario) (bulb: LightBulb) :float<USD/lm> =
        ((TheoreticalReplacementCost s bulb) + (UsageCost s bulb))/bulb.Lumens

