# Configuration

A C# library for strongly typed Configuration with validation

[![Quality Gate](https://sonarqube.com/api/badges/gate?key=res.configuration)](https://sonarqube.com/dashboard/index/res.configuration)

Example Usage (`Configuration` is the class provided by this repo)

```csharp
public class EconomicModelConfiguration, ICurrencyConversionConfiguration, IConcreteCostConfiguration 
{
    readonly Configuration configuration;
    
    public EconomicModelConfiguration(Configuration configuration) {
        Contract.Requires(configuration != null);

        this.configuration = configuration;
    
        Validate();
    }
    
    void Validate() {
        using (var validator = configuration.CreateValidator) {
            validator.Check(() => DefaultCurrency);
            validator.Check(() => DefaultConcreteCost);
        }
    }

    public string DefaultCurrency => 
        configuration.GetEnum<Currency>(MethodBase.GetCurrentMethod());

    public double DefaultConcreteCost => 
        GetDouble(MethodBase.GetCurrentMethod());
}
```