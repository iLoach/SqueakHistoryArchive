"Change Set:		8120Traits-ar.243Traits-ar.243:Remove support for isolation layers."!!TCompilingBehavior methodsFor: 'compiling' stamp: 'ar 11/12/2009 01:10'!compileAllFrom: oldClass	"Compile all the methods in the receiver's method dictionary.	This validates sourceCode and variable references and forces	all methods to use the current bytecode set"	"ar 7/10/1999: Use oldClass selectors not self selectors"		oldClass selectorsDo: [:sel | self recompile: sel from: oldClass].! !