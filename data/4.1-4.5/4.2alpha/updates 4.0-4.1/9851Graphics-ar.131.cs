"Change Set:		9851Graphics-ar.131Graphics-ar.131:Merging Graphics-bp.130:fix comment in Rectangle>>extentGraphics-ar.128:Generalize #lighter, #darker, #dimmed into #collectColors: and #collectPixels: which now also ensures that the form is unhibernated (this could cause problems during/after image same) and that we only do this for depth 32 as the other form depths are currently not implemented.Graphics-nice.129:1) Fix for http://bugs.squeak.org/view.php?id=74832) Fix _ assigments in class commentsGraphics-bp.130:fix comment in Rectangle>>extentGraphics-ar.130:Fix http://bugs.squeak.org/view.php?id=7492"!ImageReadWriter subclass: #PNMReadWriter	instanceVariableNames: 'first type origin cols rows depth maxValue tupleType pragma'	classVariableNames: ''	poolDictionaries: ''	category: 'Graphics-Files'!Pen subclass: #PenPointRecorder	instanceVariableNames: 'points'	classVariableNames: ''	poolDictionaries: ''	category: 'Graphics-Primitives'!!Form methodsFor: 'converting' stamp: 'ar 3/28/2010 15:35'!lighter	"Answer a lighter variant of this form"	^self collectColors:[:color| color lighter lighter].! !!Form methodsFor: 'converting' stamp: 'ar 3/28/2010 15:34'!collectColors: aBlock	"Create a new copy of the receiver with all the colors transformed by aBlock"	^self collectPixels:[:pv|		(aBlock value: (Color colorFromPixelValue: pv depth: self depth)) 			pixelValueForDepth: self depth.	].! !!DisplayScreen class methodsFor: 'display box access' stamp: 'wiz 5/10/2008 19:46'!depth: depthInteger width: widthInteger height: heightInteger fullscreen: aBoolean	"Force Squeak's window (if there's one) into a new size and depth."	"DisplayScreen depth: 8 width: 1024 height: 768 fullscreen: false"	<primitive: 92>	self primitiveFailed! !!Form methodsFor: 'converting' stamp: 'ar 3/28/2010 15:35'!dimmed	"Answer a dimmed variant of this form."	^self collectColors:[:color| (color alpha: (color alpha min: 0.2)) ]! !!Form methodsFor: 'converting' stamp: 'ar 4/2/2010 22:35'!collectPixels: aBlock	"Create a new copy of the receiver with all the pixels transformed by aBlock"	self depth = 32 ifFalse:[		"Perform the operation in 32bpp"		^((self asFormOfDepth: 32) collectPixels: aBlock) asFormOfDepth: self depth].	self unhibernate. "ensure unhibernated before touching bits"	^Form 		extent: self extent 		depth: self depth		bits: (self bits collect: aBlock)! !!Rectangle methodsFor: 'accessing' stamp: 'bp 4/2/2010 21:00'!extent	"Answer a point with the receiver's 	width @ the receiver's height."	^corner - origin! !!Form methodsFor: 'converting' stamp: 'ar 3/28/2010 15:35'!darker	"Answer a darker variant of this form."	^self collectColors:[:color| color darker darker]! !