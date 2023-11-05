# Considerations in writing tests

* avoid duplication of tests
* avoid complex setups (KISS) and keep them as minimal as possible
* avoid unneeded complexity and mock away dependencies that do not benefit to the logic to test
* readable and test-value should be obvious
* naming conventions are important. SINUS has a hard dependency on the naming (Given-When-Then; making the tests readable to a broader audience) and checks these (opinionated naming convention).
* use correct Assertion-method. SINUS introduces a dependency to FluentAssertions and adds some out-of-the-box fluent checks for the actual values.
* SINUS supports single/multiple assertion per Then-block while also allows multiple then blocks isolating the error to exactly one then block making it easy to stick to the 1-Then-Block-equals-1-Assertion-Pattern.
* Parameters using DynamicData is supported with the requirement that the last data-parameter represents the scenario-name.
* use Parameters to provide test-data; Opinionated: Test-Data Generation can be an own capability and might be encapsulated into an own class if complex.
* don't make internals public, but create your own test-project for the internals
