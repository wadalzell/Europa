/ /   U n i t y   b u i l t - i n   s h a d e r   s o u r c e .   C o p y r i g h t   ( c )   2 0 1 6   U n i t y   T e c h n o l o g i e s .   M I T   l i c e n s e   ( s e e   l i c e n s e . t x t ) 
 
 / /   S i m p l i f i e d   S k y b o x   s h a d e r .   D i f f e r e n c e s   f r o m   r e g u l a r   S k y b o x   o n e : 
 / /   -   n o   t i n t   c o l o r 
 
 S h a d e r   " S k y b o x / C l e a r   6   S i d e d   2 "   { 
 P r o p e r t i e s   { 
         _ F r o n t T e x   ( " F r o n t   ( + Z ) " ,   2 D )   =   " w h i t e "   { } 
         _ B a c k T e x   ( " B a c k   ( - Z ) " ,   2 D )   =   " w h i t e "   { } 
         _ L e f t T e x   ( " L e f t   ( + X ) " ,   2 D )   =   " w h i t e "   { } 
         _ R i g h t T e x   ( " R i g h t   ( - X ) " ,   2 D )   =   " w h i t e "   { } 
         _ U p T e x   ( " U p   ( + Y ) " ,   2 D )   =   " w h i t e "   { } 
         _ D o w n T e x   ( " D o w n   ( - Y ) " ,   2 D )   =   " w h i t e "   { } 
 } 
 
 S u b S h a d e r   { 
         T a g s   {   " Q u e u e " = " B a c k g r o u n d "   " R e n d e r T y p e " = " B a c k g r o u n d "   " P r e v i e w T y p e " = " S k y b o x "   } 
         C u l l   O f f   Z W r i t e   O f f   F o g   {   M o d e   O f f   } 
         P a s s   { 
                 S e t T e x t u r e   [ _ F r o n t T e x ]   {   c o m b i n e   t e x t u r e   } 
         } 
         P a s s   { 
                 S e t T e x t u r e   [ _ B a c k T e x ]     {   c o m b i n e   t e x t u r e   } 
         } 
         P a s s   { 
                 S e t T e x t u r e   [ _ L e f t T e x ]     {   c o m b i n e   t e x t u r e   } 
         } 
         P a s s   { 
                 S e t T e x t u r e   [ _ R i g h t T e x ]   {   c o m b i n e   t e x t u r e   } 
         } 
         P a s s   { 
                 S e t T e x t u r e   [ _ U p T e x ]         {   c o m b i n e   t e x t u r e   } 
         } 
         P a s s   { 
                 S e t T e x t u r e   [ _ D o w n T e x ]     {   c o m b i n e   t e x t u r e   } 
         } 
 } 
 
 F a l l b a c k   O f f 
 } 
 