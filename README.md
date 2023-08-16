# Configuration

A C# library for strongly typed Configuration with validation. Please see [Taming Configuration in C#](https://medium.com/hackernoon/taming-configuration-in-c-a2706b2d4741) for more details.

[![](https://github.com/resgroup/configuration/actions/workflows/build-and-test-RES.Configuration.yml/badge.svg)](https://github.com/resgroup/configuration/actions/workflows/build-and-test-RES.Configuration.yml) [![](https://github.com/resgroup/configuration/wiki/RES.Configuration-test-badge.svg)](https://github.com/resgroup/configuration/wiki/RES.Configuration-test-results) [![](https://github.com/resgroup/configuration/wiki/RES.Configuration-coverage-badge.svg)](https://github.com/resgroup/configuration/wiki/RES.Configuration-coverage-results)

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
        configuration.GetDouble(MethodBase.GetCurrentMethod());
}
```
