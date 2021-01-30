# TextAnimator

Text animation asset made by Dominik Beens using  
https://github.com/mdechatech/CharTweener

## Usage

Put the TextAnimator component on an object with a TextMeshProUGUI component and you're good to go.   
Text can be animated on awake or by calling Show() / Show(text) on the TextAnimator component.   

## Tags

Notes:   
Put tags directly before and after words. E.g. '<<span>tag</span>>text'.   
Tags can be combined with a comma. E.g. '<tag,tag2>'   
Valid text animation strengths are: nothing, 2 and 3.

### Punch   
<<span>punch> </punch</span>>   
This will punch the scale of a character.

### Shake
<<span>shake> </shake</span>>   
This will shake the position of a character.

### Breathe
<<span>breathe> </breathe</span>>   
This will continuously breathe the scale of a character in and out.

### Wave
<<span>wave> </wave</span>>   
This will continuously move the position of a character up and down.

### Fade
<<span>fade> </fade</span>>   
This will fade the alpha of a character in and out.

### ScaleIn
<<span>scalein> </scalein</span>>   
This will scale the the character in from 0 while stretching it along its y axis.

### Speed
<<span>speed[amount]> </speed</span>>   
<<span>speed1.5</span>> will set the text reveal speed to one and a half second per character set.   

### Pause   
<<span>pause[amount]</span>>   
<<span>pause1</span>> will pause the text reveal sequence for one second.