'From Squeak3.11alpha of 13 February 2010 [latest update: #9483] on 9 March 2010 at 11:11:23 am'!!TheWorldMenu methodsFor: 'commands' stamp: 'laza 3/5/2010 19:47'!loadProject	| stdFileMenuResult path |	"Put up a Menu and let the user choose a '.project' file to load.  Create a thumbnail and jump into the project."	Project canWeLoadAProjectNow ifFalse: [^ self].	path := FileList2 modalFolderSelector.	path ifNil: [^ nil].	stdFileMenuResult := ((StandardFileMenu new) pattern: '*.pr'; 		oldFileFrom: path) 			startUpWithCaption: 'Select a File:' translated.	stdFileMenuResult ifNil: [^ nil].	ProjectLoading 		openFromDirectory: stdFileMenuResult directory 		andFileName: stdFileMenuResult name! !