Rhino Mocks
======================================================================

[Downloads for .Net 4.0](https://github.com/alaendle/rhino-mocks/downloads)

_Version: 3.6.3.0 (vNext - unreleased)_

* Fixed corner case bug in AnyArgsExpectation - even this expectation should verify that the given arguments are valid for the methods signature.
* Fixed property behavior for internal properties ([patched by Jorge Martines](https://github.com/jorgehmv/rhino-mocks/commit/24abfef37e4c0e66d140f246013a5df5eddbc932)).
* Harmonized handling of null parameter in MockRepository.GenerateMock/GeneratePartialMock/GeneratePartialStub(Type, Type[], params object[]) to follow MockRepsoitory.GenerateStrictMock(Type type, Type[] extraTypes, params object[] argumentsForConstructor).
* Do not allow multiple return/throw/do/never options in one expect/stub call.
* Marking parts of the old classic/using/fluent syntax as deprectated - to enforce AAA style ([Rhino Mocks AAA Syntax Quick Reference](http://www.scribd.com/doc/49587062/RhinoMocksAAAQuickReference)).
* Adapted copyright information for the year 2015.
* Fixed ref parameters on COM interfaces. The value of a variable used as a ref parameter should be used as a constraint on an expectation even when if it is marked with an InteropServices.OutAttribute ([patched by andriniaina](https://github.com/andriniaina/rhino-mocks/commit/e707bdfddabb49b573e41afad82403e89c99ab2c)).
* Better support generic methods ([reported by Steinkauz](https://groups.google.com/forum/?fromgroups=#!topic/RhinoMocks/gta6a6bHhT8)).
* Updated to Castle.Core 3.2.2 (for .Net 4.0 Client Profile).
* Fixed corner case bug in AllPropertiesMatchConstraint (MemberInfo.MetadataToken can differ even on identical types).
* Updated psake to trunk version (https://github.com/psake/psake - a20bd7f52f20cc60a6b885fb02cfa1f492cec9a4).
* Updated xunit to v1.9.2.
* Update ILmerge to v2.13.
* Adapted merge process to be compatible with .Net 4.0 (even if .Net 4.5 is installed on the build machine).

_Version: 3.6.2.0_

* Better encapsulation of the ordered AAA API.
* Fixed corner case bug when working with nested recorders ([reported by honggoff](https://groups.google.com/d/topic/rhinomocks/tMAbfs2qBec/discussion)).
* Avoid unnecessary first chance NotImplementedExcption if abstract method is called in the constructor of a mocked object.
* Fixed bug introduced during Castle.Core update (affects v3.6.1).
* Applied patch for fixing event raiser to be callable with nullable arguments from romanfq.
* Updated xunit to v1.9.1.

_Version: 3.6.1.0_

* Added support for (generic) multi stubs.
* Applied patch for ordered AAA syntax from Kenneth Xu. [See his blog](http://kennethxu.blogspot.com/2009/06/rhinomocks-ordered-expectations.html)
* Updated to Castle.Core 3.0.0.
* Removed depricated API parts.
* Various bug fixes.

## Links/Resources

* [Rhino Mocks Wiki](http://www.ayende.com/wiki/Rhino+Mocks.ashx "Rhino Mocks Wiki")
* [Rhino Mocks 4.0 Planning](http://nhprof.uservoice.com/pages/28152-rhino-mocks-4-0 "Rhino 4.0 Planning")