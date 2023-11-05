# Strategy

Creating automated tests makes a lot of sense, because it enables you to get feedback
about your development process even before you hand over the application to the
quality assurance and later your customers (especially true if you change existing functionality).
In software engineering we refer here to the term "shift-left" or "fail-fast", what is an
ultimately good thing and improvement in the industry of software development.

* Create a lot of unit tests.
  * Opinionated: SINUS can be your one-stop-shop library for testing
    based on MS-Test in a nice BDT API, but consider that you will introduce also
    some dependencies to libraries that need to considered (e.g.: Selenium).
  * Opinionated: track code coverage automatically to be confident about the amount of tests.
    Herefore use CI/CD analysis and quality gates, but consider also local analysis.
    https://github.com/danielpalme/ReportGenerator
* create some service tests.
  * Opinionated: SINUS will help you with that task as well, but it might be
    beneficial for the overall process to create a small UI to test. A lot
    of applications have these capabilities via Swagger/OpenAPI-Frontends.
    Hereby we see methods that instrument the code that is checked by the integration test to be able to evaluate the code coverage.
    https://learn.microsoft.com/en-us/visualstudio/test/microsoft-code-coverage-console-tool?view=vs-2022
* create few UI tests.
  * Opinionated: SINUS is primarily built for this task, but use the amount of
    tests with caution. UIs are constantly changing, so keep your IDs to capture
    to reduce the amount of test refactoring effort.

Quality criterias are:

* fully automated (including result interpretation)
* has full control
* isolated (preferred in-memory)
  * parallelizable
* stable and consistent (not flaky) in its result
* running fast
* testing a single concept in the system
* readable
* performant
* maintainable
* trustworthy

Consider:

* standard flow (positive cases)
* edge cases
* failing flows (negative cases)

(see equivalence partitioning)

Unit tests should be "FIRST" (fast, independent, repeatable, self-validating, thorough) and RITE (readable, isolated, thorough, explicit).

Do not test:

* bootstrap code (container registration, initialization)
* configuration (constants, enums, readonly fields)
* model classes and data transfer objects (DTO)
* language / framework features of the programming environment
* functions that directly return variables
* facades without any logic
* private methods
* Finalizers (especially in combination with IDisposable pattern implementation)
* exception messages and log messages

To sum it up, we are testing execution logic that can be called from outside of the unit without depending on the internal setup and implementation.
