   M S B A M L   `   `   `  ����O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  5   	   J FPharmacy_client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:Pharmacy_clientPharmacy_client 5   	   I2clr-namespace:System.Collections;assembly=mscorlibSystem.Collections  5   	      Pharmacy_client.UserWnd   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   9system&clr-namespace:System;assembly=mscorlib     5   	   /localclr-namespace:Pharmacy_client     5   	   Jcollections2clr-namespace:System.Collections;assembly=mscorlib     5   	   -   5          � Title$  UserWnd��5   	   $	��208��6   $	��424��6&    � 
ResizeMode$ NoResize=�6n    � WindowStyle$
 None=�6�    � Triggers 5	      :� 5
   
    :� RoutedEvent$ Window.Loaded��6   [�5      �� 5      .��5      �� .��5      W�  W� From$ 0X�6*   	 W� To$ 1X�63    g� Duration$ 0:0:0.5D�6:    �� 
TargetName$ TabCtrl��5   +   	 �� TargetProperty$	 Opacity�5   +   5      5      5      .��5      � -   5           TitleBarBackgroundBrush#���   5      .+�5   
   �� -   5        
 ��Name$
 CloseButton��5      $
��Left=�6&   $��	389,0,0,0q�6A   $	��Top=�6T   $��34��6l   $��26��6w   
������ 
������  K� ZIndex$ 1��6�    # Vs2012WindowStyleTitleBarButton#��[ 6�   .��5      � $��9��6   $��9��6!   .+�5      F� $	��1.5��5       �� Stretch$
 None=�5      +����        � z  z �     z � z     	��5      ��  �� RelativeSource ��  �� AncestorType$ ButtonL� �� Path$ 
Foreground�5      5      5       �� 5!   
   -   5        $
 	MinButton��5!      $
��Left=�6$   $��	355,0,0,0q�6?   $	��Top=�6R   $��34��6j   $��26��6u   
������ 
������ $ 1��6�   #��[ 6�   .��5"      � $��9��6   $��9��6!   .+�5#      F� $��1��5%      $
 None=�5&       �� EdgeMode$ Aliased=�5'      =����     z � z  z �    ��j � z ��j �    ��[ � z ��[ 	��5$      ��  �� $ ButtonL�$ 
Foreground�5(      5*      5+      �� 5,   
   -   5         ��Name$ TabCtrl��5,      $
��Left=�6&   $	��200��6A   $	��Top=�6N   $	��426��6f     
TabCtrlCol#���  6r   #���  6�   $��-1,9,0,0q�6�   .@�5-   
   �� $��Products��6     
ItemTabCol#���  6$     ItemTabBorderCol#���  6N   $��1,0,0,-3q�6     ItemTabTextCol#���  6�     TabItemStyle#���  6�   .��5.      � ���	#FFE5E5E5.+�5/      �� -   5        $
 CartBtn��5/      $��Add to Cart��6*   $
��Left=�6@   $��
333,10,0,0q�6[   $	��Top=�6o   $��20��6�   $��75��6�     BtnStyle#���  6�   �� 50      -   5         ��Name$ CountTb��50      $
��Left=�6+   $��22��6F   $��
373,35,0,0q�6R    �� TextWrapping$
 Wrap=�6f   $��1��6z    �� 	MaxLength$ 3��6�   $	��Top=�6�   $��35��6�   6�   �� 51      $��Count:��6   $
��Left=�6)   $��
328,31,0,0q�6D   $	��Top=�6X   $
��Bold"�6p   �� 52      -   5         ��Name$ ProductsLbox��52      $
��Left=�60   $	��170��6K   $	��Top=�6X   $	��123��6p   6|   �� 53      $��Product:��6   $
��Left=�6+   $��
128,10,0,0q�6F   $	��Top=�6Z   $
��Bold"�6r   �� 54      $��Vendor:��6   $
��Left=�6*   $��
128,35,0,0q�6E   $	��Top=�6Y   $
��Bold"�6q   �� 55      $��Available for purchase:��6   $
��Left=�6:   $��
128,61,0,0q�6U   $	��Top=�6i   $
��Bold"�6�   �� 56      $��Year:��6   $
��Left=�6(   $��
128,87,0,0q�6C   $	��Top=�6W   $
��Bold"�6o   �� 57      $��Description:��6   $
��Left=�6/   $��128,113,0,0q�6J   $	��Top=�6_   $
��Bold"�6w   �� 58      -	   5         ��Name$ 	ProductLb��58      $�� ��6+   $
��Left=�67   $��
182,10,0,0q�6R   $	��Top=�6f   �� 59      -
   5        $ VendorLb��59      $�� ��6*   $
��Left=�66   $��
182,36,0,0q�6Q   $	��Top=�6e   �� 5:      -   5        $ PurchLb��5:      $�� ��6)   $
��Left=�65   $��
263,61,0,0q�6P   $	��Top=�6d   �� 5;      -   5        $ YearLb��5;      $�� ��6(   $
��Left=�64   $��
169,87,0,0q�6O   $	��Top=�6c   �� 5<      -   5        $ DescrLb��5<      $�� ��6)   $
��Left=�65   $��206,113,0,0q�6P   $	��Top=�6e   �� 5=      $��Price:��6   $
��Left=�6)   $��
328,57,0,0q�6D   $	��Top=�6X   $
��Bold"�6p   �� 5>      -   5        $ PriceLb��5>      $�� ��6)   $
��Left=�65   $��
369,57,0,0q�6P   $	��Top=�6d   5@      5A      �� 5B   
   $
��Cart��6   #���  6    #���  6J   $��1,0,0,-3q�6{   #���  6�   #���  6�   .��5C      � ���	#FFE5E5E5.+�5D      �� -   5        $
 BuyBtn��5D      $	��Buy��6-   $��20��6;   $
��Left=�6G   $��333,140,0,0q�6b   $	��Top=�6w   $��75��6�   #���  6�   &   System.Windows.Controls.DataGrid  5E      -   5          Name$ CartData��5E      $
��Left=�6-   $	��Top=�6H    J� RenderTransformOrigin$ 	0.5,0.538&�6`   $	��135��6�   $	��424��6�      AutoGenerateColumns .     CanUserReorderColumns .     CanUserResizeColumns .     CanUserResizeRows .     CanUserSortColumns .  "   VerticalScrollBarVisibility$ Visible=�6'     
IsReadOnly .     SelectionMode$  Single=�6�  $��-1,0,0,0q�6�  !   Columns! 5F      0  *System.Windows.Controls.DataGridTextColumn  5G      ,  &System.Windows.Controls.DataGridColumn"   Header$" Product��6o   #   Width5  /System.Windows.Controls.DataGridLengthConverter$	# 198 6�   1  +System.Windows.Controls.DataGridBoundColumn$   Binding$ 6-   �� *
ProductBnd+%   ClipboardContentBinding% 6L   Z� 6�     5H      $" Price��6m   $# 67 6|   $ 6-   �� *
PriceBnd+% 6J   Z� 6|     5I      $" Count��6m   $# 67 6|   $ 6-   �� *
CountBnd+% 6J   Z� 6|     5J      $	" Sum��6k   $# 67 6x   &   CanUserReorder& . $ 6-   �� *SumBnd+% 6H   Z� 6�   5L      �� 5M      -   5        $
 DelBtn��5M      $��20��6-   $��Delete��69   $
��Left=�6J   $��
10,140,0,0q�6e   $	��Top=�6y   $��75��6�   #���  6�   �� 5N      $��Total:��6   $
��Left=�6)   $��
90,137,0,0q�6D   $	��Top=�6X   �� 5O      -   5        $ TotSumLb��5O      $��0��6+   $
��Left=�67   $��133,137,0,0q�6R   $	��Top=�6g   5P      5Q      5R      5T      5U      