"Change Set:		8354KernelTests-ar.117KernelTests-ar.117:AllocationTests covering the behavior on excessive allocations.KernelTests-ar.113:Add tests for compact class index of LPI and LNI.KernelTests-nice.114:Tests for Float characterization messagesKernelTests-ul.115:- tests for Behavior >> #allSelectorsBelow: and Behavior >> #allSelectorsKernelTests-ul.116:- fix BehaviorTest >> #testAllSelectorsBelow "!TestCase subclass: #AllocationTest	instanceVariableNames: ''	classVariableNames: ''	poolDictionaries: ''	category: 'KernelTests-Objects'!!AllocationTest methodsFor: 'tests' stamp: 'ar 12/4/2009 14:08'!testOneGigAllocation	"Documentating a weird bug in the allocator"	| sz array failed |	failed := false.	sz := 1024*1024*1024.	array := [Array new: sz] on: OutOfMemory do:[:ex| failed := true].	self assert: (failed or:[array size = sz]).	! !!BehaviorTest methodsFor: 'tests' stamp: 'ul 12/3/2009 04:56'!testAllSelectors	self assert: ProtoObject allSelectors = ProtoObject selectors asIdentitySet.	self assert: Object allSelectors = (Object selectors union: ProtoObject selectors) asIdentitySet.! !!FloatTest methodsFor: 'characterization' stamp: 'nice 6/11/2009 20:47'!testCharacterization	"Test the largest finite representable floating point value"	self assert: Float fmax successor = Float infinity.	self assert: Float infinity predecessor = Float fmax.	self assert: Float fmax negated predecessor = Float infinity negated.	self assert: Float infinity negated successor = Float fmax negated.		"Test the smallest positive representable floating point value"	self assert: Float fmin predecessor = 0.0.	self assert: 0.0 successor = Float fmin.	self assert: Float fmin negated successor = 0.0.	self assert: 0.0 predecessor = Float fmin negated.		"Test the relative precision"	self assert: Float one + Float epsilon > Float one.	self assert: Float one + Float epsilon = Float one successor.	self assert: Float one + (Float epsilon / Float radix) = Float one.		"Test maximum and minimum exponent"	self assert: Float fmax exponent = Float emax.	self assert: Float fminNormalized exponent = Float emin.	Float denormalized ifTrue: [		self assert: Float fminDenormalized exponent = (Float emin + 1 - Float precision)].		"Alternative tests for maximum and minimum"	self assert: (Float radix - Float epsilon) * (Float radix raisedTo: Float emax) = Float fmax.	self assert: Float epsilon * (Float radix raisedTo: Float emin) = Float fmin.		"Test sucessors and predecessors"	self assert: Float one predecessor successor = Float one.	self assert: Float one successor predecessor = Float one.	self assert: Float one negated predecessor successor = Float one negated.	self assert: Float one negated successor predecessor = Float one negated.	self assert: Float infinity successor = Float infinity.	self assert: Float infinity negated predecessor = Float infinity negated.	self assert: Float nan predecessor isNaN.	self assert: Float nan successor isNaN.		"SPECIFIC FOR IEEE 754 double precision - 64 bits"	self assert: Float fmax hex = '7FEFFFFFFFFFFFFF'.	self assert: Float fminDenormalized hex = '0000000000000001'.	self assert: Float fminNormalized hex = '0010000000000000'.	self assert: 0.0 hex = '0000000000000000'.	self assert: Float negativeZero hex = '8000000000000000'.	self assert: Float one hex = '3FF0000000000000'.	self assert: Float infinity hex = '7FF0000000000000'.	self assert: Float infinity negated hex = 'FFF0000000000000'.! !!BehaviorTest methodsFor: 'tests' stamp: 'ul 12/3/2009 05:39'!testAllSelectorsBelow	self assert: (Object allSelectorsBelow: ProtoObject) = Object selectors asIdentitySet.	self assert: (Object allSelectorsBelow: nil) = (Object selectors union: ProtoObject selectors) asIdentitySet! !!LargePositiveIntegerTest methodsFor: 'tests' stamp: 'ar 11/30/2009 22:06'!testCompactClassIndex	self assert: LargePositiveInteger indexIfCompact = 5.! !!LargeNegativeIntegerTest methodsFor: 'tests' stamp: 'ar 11/30/2009 22:06'!testCompactClassIndex	self assert: LargeNegativeInteger indexIfCompact = 4.! !!AllocationTest methodsFor: 'tests' stamp: 'ar 12/4/2009 14:08'!testOneMegAllocation	"Documentating a weird bug in the allocator"	| sz array failed |	failed := false.	sz := 1024*1024.	array := [Array new: sz] on: OutOfMemory do:[:ex| failed := true].	self assert: (failed or:[array size = sz]).	! !!AllocationTest methodsFor: 'tests' stamp: 'ar 12/4/2009 14:08'!testOutOfMemorySignal	"Ensure that OOM is signaled eventually"	| sz |	sz := 512*1024*1024. "work around the 1GB alloc bug"	self should:[2000 timesRepeat:[Array new: sz]] raise: OutOfMemory.	"Call me when this test fails, I want your machine"	sz := 1024*1024*1024*1024.	self should:[Array new: sz] raise: OutOfMemory.! !