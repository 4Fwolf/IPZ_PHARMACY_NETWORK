   M S B A M L   `   `   `  ����\  XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089   	TabBorder  �� Color  TabText  
TabBackCol  SelectedTabBackCol  TabPanelBack  ContentBorder 
 SepCol  SelTabHigh1  SelTabHigh2 	 UnsTabHighCol2 
 UnsTabHighlCol1  UnsTabBackCol  TabControlStyle �� 
TargetType �� Property �� Value �� 
TargetType-    'System.Windows.Input.KeyboardNavigation    TabNavigation � RowDefinitions ��Name K� ZIndex	 K� IsItemsHost
    TabIndex ��Name �� CornerRadius    DirectionalNavigation ��Name  TabItemStyle  TextCol � 	Resources  Key �� 
TargetName �� TargetProperty W� From	 W� To g� Duration �Name � ColumnDefinitions  
ItemTabCol  Vs2012WindowBorderBrush  UnsTabHighCol1 �� Triggers P� Property P� Value �� 
TargetName N� EnterActions  StartAction N� ExitActions  
ExitAction  MyFocusVisualStyte�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     :x,http://schemas.microsoft.com/winfx/2006/xaml      5      %�  &	        &	 #     &	 G     &	 j     &	 �     &	 �     &	 �     &	 �     &	     &		 @    &	
 ^    &	 }    &	 �    &	 �    7  7  &	 �    �� 5      $  	#FF3F3F46��6(   �� 5      $  
GhostWhite��6&   �� 5      $  	#FF3F3F46��6)   �� 5      $  	#FF3F3F46��61   �� 5	      $  Transparent��6+   �� 5
      $  Transparent��6,   �� 5      $  	OrangeRed��6%   �� 5      $  	OrangeRed��6*   �� 5      $  	OrangeRed��6*   �� 5      $
  Gray��6-   �� 5      $  White��6.   �� 5      $  Transparent��6,   �� 5      " ��6$   .�5   
   ��  �@�OverridesDefaultStyle . 62   �� 5   
    �@J�SnapsToDevicePixels . 60   �� 5   
    � �� 5      �� 5      " ��6"   .R�5      � $ Local=�6    5      �� 5      $
��Auto �6,   �� 5       $��* �6,   .+�5"      �� $ HeaderPanel��6#   $��0��5#      $ 1��5$      $��2,1,4,-1q�5%      	 . $
 1��5'      #���  5(      �� 5)      $ Border��6!   $��1��5*      $��2,0,2,2q�5+      $ 2��5,      $ Local=�5-      $ 	Contained=�5.      $
 2��5/      #���  50      ._�51      �� $ PART_SelectedContentHost��6/   $��SelectedContent��52   /   53      54      55      57      58      �� 5:      " ��6!   .�5;   
   ��  � ��$ 12(�6%   �� 5<   
    � ��$ Normal"�6'   �� 5=   
    � ��# �  6'   �� 5>   
    �@�FocusVisualStyle 6-   �� *MyFocusVisualStyte+�� 5?   
    � �� 5@      �� 5A      " ��6"    5B      ���� 5C      StartAction .��5D      W� $ Border1Highlited��6.   $ (UIElement.Opacity)�6W   $ 0X�6�   $ 1X�6�   $ 0:0:0.5D�6�   W� 5E      $ Border2Highlited��6.   $ (UIElement.Opacity)�6W   $ 0X�6�   $ 1X�6�   $ 0:0:0.5D�6�   5F      �� 5H      
ExitAction .��5I      W� $ Border1Highlited��6.   $ Opacity�6W   $ 1X�6{   $ 0X�6�   $ 0:0:0.3D�6�   W� 5J      $ Border2Highlited��6.   $ Opacity�6W   $ 1X�6{   $ 0X�6�   $ 0:0:0.3D�6�   5K      5L      .R�5N      � $
 Root��6    5O      �� 5P      $��* �6/   �� 5Q      $��1 �6/    5S      �� 5T      $��* �6,   �� 5U      $��6 �6,   �� 5V      $��2 �6,   .+�5X      �� $ Border��6!   $��	0,0,-1,-5q�5Y   !   #���  5Z   !   ._�5\      �� $ ContentSite��6/   $��Center=�5]   +   $��Center=�5^   +   $��Header��5_   +   $��5,2,5,2q�5`   +   ��. 5a   +   5b      �� 5c      $��1��6!   $��0,10,0,4q�61   #���  6C   �� 5e      $��0,0,0,-5q�6!   $��1��63   $��1��6>   $��2��6K   #��� 	 6_   �� 5f      $��Border1Highlited��6_   $��0,0,0,-5q�6!   $��1��63   $��1��6>   $��2��6K   #���  6w   �� 5h      $��2��6!   $��2��6.   #��� 	 6B   �� 5i      $��Border2Highlited��6B   $��2��6!   $��2��6.   #��� 	 6Z   5k       5l      P� 5m       �@��
IsSelected . . �5n      ��  �@K�ZIndex$	 100��6=   �� 5o      $ Border1Highlited��6%   # �  6Y    � ���� 5p      $ Border2Highlited��6%   # �  6Y    � ���� 5q      $ Border��6%   # �  6O    � �� 5r      �� 5s   "   8��  62    5u      �� 5v   "   8�� 62   5x      P� 5y       � {� .  . �5z      �� # �  6;    � ��5{      5}      5      5�      �� 5�      .�5�   
   ��  � �� 5�      �� 5�      .R�5�      �� $ 
FocusStyle��6   ���	#4433b4ed61   5�      5�      5�      5�      