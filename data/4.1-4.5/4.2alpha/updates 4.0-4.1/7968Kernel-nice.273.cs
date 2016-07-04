"Change Set:		7968Kernel-nice.273Kernel-nice.273:remove uncessary sends of #valuesbecause (aDictionary values do:) is same as (aDictionary do:)except when self modified in the do loop.Kernel-nice.269:The so-called Eliot solution to the xor: problemKernel-nice.270:Fix MNU #empty while DebuggingKernel-ar.271:Merging Kernel-ul.270:- faster DateAndTime and Date creation with #year:month:day: and friends- minor formatting fixesKernel-nice.272:How to gain a factor x2 on speed of String streamContents: [:strm | Float pi absPrintExactlyOn: strm base: 10]1) do not use d := r // s.  r := r \\ s. This will evaluate the quotient twice.2) s generally has a lot of trailing bits = 0, so it is faster to evaluate:d := (r >> (s lowBit - 1)) // (s >> (s lowBit - 1)).especially s lowBit - 1 and (s >> (s lowBit - 1)) can go outside the while loop3) revert the tests in tc1 and tc2: these tests are false but for the last digit.Since Float usually have more than 2 digits, it is statistically faster to test for the false condition first.4) use and: or: when due instead of & |"!!DateAndTime methodsFor: 'squeak protocol' stamp: 'ul 10/15/2009 01:15'!midnight	"Answer a DateAndTime starting at midnight local time"	^self class basicNew		setJdn: jdn		seconds: 0		nano: 0		offset: self class localOffset! !!Float methodsFor: 'printing' stamp: 'nice 10/19/2009 14:28'!absPrintExactlyOn: aStream base: base	"Print my value on a stream in the given base.  Assumes that my value is strictly	positive; negative numbers, zero, and NaNs have already been handled elsewhere.	Based upon the algorithm outlined in:	Robert G. Burger and R. Kent Dybvig	Printing Floating Point Numbers Quickly and Accurately	ACM SIGPLAN 1996 Conference on Programming Language Design and Implementation	June 1996.	This version guarantees that the printed representation exactly represents my value	by using exact integer arithmetic."	| fBase significand exp baseExpEstimate be be1 r s mPlus mMinus scale roundingIncludesLimits d tc1 tc2 fixedFormat decPointCount slowbit shead |	self isInfinite ifTrue: [aStream nextPutAll: 'Infinity'. ^ self].	fBase := base asFloat.	significand := self significandAsInteger.	roundingIncludesLimits := significand even.	exp := (self exponent - 52) max: MinValLogBase2.	baseExpEstimate := (self exponent * fBase reciprocalLogBase2 - 1.0e-10) ceiling.	exp >= 0		ifTrue:			[be := 1 << exp.			significand ~= 16r10000000000000				ifTrue:					[r := significand * be * 2.					s := 2.					mPlus := be.					mMinus := be]				ifFalse:					[be1 := be * 2.					r := significand * be1 * 2.					s := 4.					mPlus := be1.					mMinus := be]]		ifFalse:			[(exp = MinValLogBase2 or: [significand ~= 16r10000000000000])				ifTrue:					[r := significand * 2.					s := (1 << (exp negated)) * 2.					mPlus := 1.					mMinus := 1]				ifFalse:					[r := significand * 4.					s := (1 << (exp negated + 1)) * 2.					mPlus := 2.					mMinus := 1]].	baseExpEstimate >= 0		ifTrue: [s := s * (base raisedToInteger: baseExpEstimate)]		ifFalse:			[scale := base raisedToInteger: baseExpEstimate negated.			r := r * scale.			mPlus := mPlus * scale.			mMinus := mMinus * scale].	((r + mPlus < s) not and: [roundingIncludesLimits or: [r + mPlus > s]])		ifTrue: [baseExpEstimate := baseExpEstimate + 1]		ifFalse:			[r := r * base.			mPlus := mPlus * base.			mMinus := mMinus * base].	(fixedFormat := baseExpEstimate between: -3 and: 6)		ifTrue:			[decPointCount := baseExpEstimate.			baseExpEstimate <= 0				ifTrue: [aStream nextPutAll: ('0.000000' truncateTo: 2 - baseExpEstimate)]]		ifFalse:			[decPointCount := 1].	slowbit := s lowBit - 1.	shead := s >> slowbit.	[d := (r >> slowbit) // shead.	r := r - (d*s).	(tc1 := (r > mMinus) not and: [roundingIncludesLimits or: [r < mMinus]]) |	(tc2 := (r + mPlus < s) not and: [roundingIncludesLimits or: [r + mPlus > s]])] whileFalse:		[aStream nextPut: (Character digitValue: d).		r := r * base.		mPlus := mPlus * base.		mMinus := mMinus * base.		decPointCount := decPointCount - 1.		decPointCount = 0 ifTrue: [aStream nextPut: $.]].	tc2 ifTrue:		[(tc1 not or: [r*2 >= s]) ifTrue: [d := d + 1]].	aStream nextPut: (Character digitValue: d).	decPointCount > 0		ifTrue:		[decPointCount - 1 to: 1 by: -1 do: [:i | aStream nextPut: $0].		aStream nextPutAll: '.0'].	fixedFormat ifFalse:		[aStream nextPut: $e.		aStream nextPutAll: (baseExpEstimate - 1) printString]! !!MethodProperties methodsFor: 'testing' stamp: 'nice 10/15/2009 00:36'!analogousCodeTo: aMethodProperties	pragmas		ifNil: [aMethodProperties pragmas notEmpty ifTrue: [^false]]		ifNotNil:			[aMethodProperties pragmas isEmpty ifTrue: [^false].			 pragmas size ~= aMethodProperties pragmas size ifTrue:				[^false].			 pragmas with: aMethodProperties pragmas do:				[:mine :others|				(mine analogousCodeTo: others) ifFalse: [^false]]].	^(self hasAtLeastTheSamePropertiesAs: aMethodProperties)	  and: [aMethodProperties hasAtLeastTheSamePropertiesAs: self]! !!DateAndTime class methodsFor: 'squeak protocol' stamp: 'ul 10/15/2009 01:19'!year: year month: month day: day hour: hour minute: minute	"Return a DateAndTime"	^self 		year: year 		month: month 		day: day 		hour: hour		minute: minute		second: 0! !!True methodsFor: 'logical operations' stamp: 'em 3/24/2009 14:05'!xor: aBoolean	"Posted by Eliot Miranda to squeak-dev on 3/24/2009"		^aBoolean not! !!False methodsFor: 'logical operations' stamp: 'em 3/24/2009 14:05'!xor: aBoolean	"Posted by Eliot Miranda to squeak-dev on 3/24/2009"	^aBoolean! !!DateAndTime class methodsFor: 'squeak protocol' stamp: 'ul 10/15/2009 01:05'!year: year month: month day: day	"Return a DateAndTime, midnight local time"		^self 		year: year 		month: month 		day: day 		hour: 0		minute: 0! !!ClassDescription methodsFor: 'private' stamp: 'nice 10/19/2009 20:36'!linesOfCode	"An approximate measure of lines of code.	Includes comments, but excludes blank lines."	| lines |	lines := self methodDict inject: 0 into: [:sum :each | sum + each linesOfCode].	self isMeta 		ifTrue: [^ lines]		ifFalse: [^ lines + self class linesOfCode]! !!DateAndTime class methodsFor: 'squeak protocol' stamp: 'ul 10/15/2009 00:58'!year: year month: month day: day hour: hour minute: minute second: second nanoSecond: nanoCount offset: offset	"Return a DateAndTime"	| monthIndex daysInMonth p q r s julianDayNumber |	monthIndex := month isInteger ifTrue: [month] ifFalse: [Month indexOfMonth: month].	daysInMonth := Month		daysInMonth: monthIndex		forYear: year.	day < 1 ifTrue: [self error: 'day may not be zero or negative'].	day > daysInMonth ifTrue: [self error: 'day is after month ends']. 			p := (monthIndex - 14) quo: 12.	q := year + 4800 + p.	r := monthIndex - 2 - (12 * p).	s := (year + 4900 + p) quo: 100.	julianDayNumber := 		( (1461 * q) quo: 4 ) +			( (367 * r) quo: 12 ) - 				( (3 * s) quo: 4 ) + 					( day - 32075 ).	^self basicNew		setJdn: julianDayNumber 		seconds: hour * 60 + minute * 60 + second		nano: nanoCount		offset: offset;		yourself! !Boolean removeSelector: #xor:!