   M S B A M L   `   `   `  ���� O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  J FPharmacy_client, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:Pharmacy_clientPharmacy_client I2clr-namespace:System.Collections;assembly=mscorlibSystem.Collections     Pharmacy_client.UserWnd   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     9system&clr-namespace:System;assembly=mscorlib     /localclr-namespace:Pharmacy_client     Jcollections2clr-namespace:System.Collections;assembly=mscorlib     -     � Title$  UserWnd��$	��208��$	��424�� � 
ResizeMode$ NoResize=� � WindowStyle$
 None=� � Triggers :�  :� RoutedEvent$ Window.Loaded��[��� .���� .��W�  W� From$ 0X�	 W� To$ 1X� g� Duration$ 0:0:0.5D� �� 
TargetName$ TabCtrl��	 �� TargetProperty$	 Opacity�.��� -      TitleBarBackgroundBrush#���   .+��� -   
 ��Name$
 CloseButton��$
��Left=�$��	389,0,0,0q�$	��Top=�$��34��$��26��
������ 
������  K� ZIndex$ 1�� # Vs2012WindowStyleTitleBarButton#��[ .��� $��9��$��9��.+�F� $	��1.5�� �� Stretch$
 None=�+����        � z  z �     z � z     	����  �� RelativeSource ��  �� AncestorType$ ButtonL� �� Path$ 
Foreground��� -   $
 	MinButton��$
��Left=�$��	355,0,0,0q�$	��Top=�$��34��$��26��
������ 
������ $ 1��#��[ .��� $��9��$��9��.+�F� $��1��$
 None=� �� EdgeMode$ Aliased=�=����     z � z  z �    ��j � z ��j �    ��[ � z ��[ 	����  �� $ ButtonL�$ 
Foreground��� -    ��Name$ TabCtrl��$
��Left=�$	��200��$	��Top=�$	��426��  
TabCtrlCol#���  #���  $��-1,9,0,0q�.@��� $��Products��  
ItemTabCol#���    ItemTabBorderCol#���  $��1,0,0,-3q�  ItemTabTextCol#���    TabItemStyle#���  .��� ���	#FFE5E5E5.+��� -   $
 CartBtn��$��Add to Cart��$
��Left=�$��
333,10,0,0q�$	��Top=�$��20��$��75��  BtnStyle#���  �� -    ��Name$ CountTb��$
��Left=�$��22��$��
373,35,0,0q� �� TextWrapping$
 Wrap=�$��1�� �� 	MaxLength$ 3��$	��Top=�$��35���� $��Count:��$
��Left=�$��
328,31,0,0q�$	��Top=�$
��Bold"��� -    ��Name$ ProductsLbox��$
��Left=�$	��170��$	��Top=�$	��123���� $��Product:��$
��Left=�$��
128,10,0,0q�$	��Top=�$
��Bold"��� $��Vendor:��$
��Left=�$��
128,35,0,0q�$	��Top=�$
��Bold"��� $��Available for purchase:��$
��Left=�$��
128,61,0,0q�$	��Top=�$
��Bold"��� $��Year:��$
��Left=�$��
128,87,0,0q�$	��Top=�$
��Bold"��� $��Description:��$
��Left=�$��128,113,0,0q�$	��Top=�$
��Bold"��� -	    ��Name$ 	ProductLb��$�� ��$
��Left=�$��
182,10,0,0q�$	��Top=��� -
   $ VendorLb��$�� ��$
��Left=�$��
182,36,0,0q�$	��Top=��� -   $ PurchLb��$�� ��$
��Left=�$��
263,61,0,0q�$	��Top=��� -   $ YearLb��$�� ��$
��Left=�$��
169,87,0,0q�$	��Top=��� -   $ DescrLb��$�� ��$
��Left=�$��206,113,0,0q�$	��Top=��� $��Price:��$
��Left=�$��
328,57,0,0q�$	��Top=�$
��Bold"��� -   $ PriceLb��$�� ��$
��Left=�$��
369,57,0,0q�$	��Top=��� $
��Cart��#���  #���  $��1,0,0,-3q�#���  #���  .��� ���	#FFE5E5E5.+��� -   $
 BuyBtn��$	��Buy��$��20��$
��Left=�$��333,140,0,0q�$	��Top=�$��75��#���  &   System.Windows.Controls.DataGrid  -     Name$ CartData��$
��Left=�$	��Top=� J� RenderTransformOrigin$ 	0.5,0.538&�$	��135��$	��424��   AutoGenerateColumns .     CanUserReorderColumns .     CanUserResizeColumns .     CanUserResizeRows .     CanUserSortColumns .  "   VerticalScrollBarVisibility$ Visible=�   
IsReadOnly .     SelectionMode$  Single=�$��-1,0,0,0q�!   Columns! 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn"   Header$" Product��#   Width5  /System.Windows.Controls.DataGridLengthConverter$	# 198 1  +System.Windows.Controls.DataGridBoundColumn$   Binding$ �� *
ProductBnd+%   ClipboardContentBinding% Z�   $" Price��$# 67 $ �� *
PriceBnd+% Z�   $" Count��$# 67 $ �� *
CountBnd+% Z�   $	" Sum��$# 67 &   CanUserReorder& . $ �� *SumBnd+% Z� �� -   $
 DelBtn��$��20��$��Delete��$
��Left=�$��
10,140,0,0q�$	��Top=�$��75��#���  �� $��Total:��$
��Left=�$��
90,137,0,0q�$	��Top=��� -   $ TotSumLb��$��0��$
��Left=�$��133,137,0,0q�$	��Top=�