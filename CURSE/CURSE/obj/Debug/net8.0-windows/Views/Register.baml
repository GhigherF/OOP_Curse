   M S B A M L   `   `   `  ����@  <CURSE, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:CURSE.ViewsCURSE.Views  5          CURSE.Views.Register   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      +localclr-namespace:CURSE.Views     5        � 	Resources  5
         ValidationErrorTemplate � ToolTip��%�   &	        �� 5   
   .R�5      ]� $
��True��6   .+�5      �� $	��⚠��6   
���  ��$��16(�5      $��2q�5       5      �� *[0].ErrorContent+�� 5      5      5      5      .��5      �  � ShowGridLines$ false��6   .+�5   
   � $��1��6   $ false��6    � ColumnDefinitions 5      �� 5      �� 5      �� 5       � RowDefinitions 5!      �� 5"      �� 5#      �� 5$      �� 5%      �� 5&      �� 5'      �� 5(      �� 5)      �� 5*      .+�5.      �� ���#0f0f0b$��3��6*   $��10��6>   6R   �� 5/      $��1��6   
�������$��50,30q�68   $��2��6G   $��20(�6X   Логин:6n   �� 50      $��1��6   $��0��6#   $��30(�63   $��3��6A   $��3��6U   $��Italic$�6g   $��80,60,80,50q�6z    =� ErrorTemplate# [  51      ��52      �� *RegisterVM.Nick+ �� UpdateSourceTrigger$ PropertyChanged=� �� ValidatesOnDataErrors$
 True�� �� NotifyOnValidationError$
 True��.��53   '   ��54      �� 55      	 �� 
TargetType"	 ��6   
 �� Triggers
 56      P� 57       P� Property �@=�HasError P� Value$
 True��6E   . �58   "   ��  �� Property �@�ToolTip �� Value 59      ��  �� RelativeSource �� *Self+ �� Path$) #(Validation.Errors)[0].ErrorContent��� 5:   "    � ��
 �  ��6@   �� 5;   "    � ��$ 2q�6D   5<      5>      .��5?   !   5B      �� 5C      $��3��6   
�������$��50,15q�68   $��18(�6I   $��3��6W   	E-mail:6r   �� 5E      $��3��6   $��0��6#   $��3��63   $��3��6G   $��80,45,80,65q�6X   $��30(�6m   $��Italic$�6{   # [  5F      ��5G      �� *RegisterVM.Email+$ PropertyChanged=�$
 True��$
 True��.��5H   '   ��5I      �� 5J      "	 ��6   
 5K      P� 5L       �@=�HasError$
 True��6E   . �5M   "   ��  �@�ToolTip 5N      ��  �� *Self+$) #(Validation.Errors)[0].ErrorContent��� 5O   "    � ��
 �  ��6@   �� 5P   "    � ��$ 2q�6D   5Q      5S      .��5T   !   5U      �� 5V      $��7��6   $��1��6"   ��5W      �� *RegisterUser+{�5X   
   �� *RegisterVM.CanRegister+.��67   Регистрация6D   �� 5Y      
�������$��5��6+   $��20(�68   $��50 0 0 0q�6F   Пароль:6a   G� 5Z      -   5        $��5��5Z      $��0��6'   $��3��67   $��3��6K   $��80,35,80,75q�6\   $��30(�6q   $��Italic$�6   # [  5a      
 � Tag 5]      �� *RegisterVM.Password+$ PropertyChanged=�$
 True��$
 True����5b      �� 5c      "	 G�6   
 5d      P� 5e       �@=�HasError$
 True��6E   . �5f   "   ��  �@�ToolTip 5g      ��  �� *Self+$) #(Validation.Errors)[0].ErrorContent��� 5h   "    � ��
 �  ��6@   �� 5i   "    � ��$ 2q�6D   5j      5l      5n      �� 5o      $��9��6   $��1��6%   $��вход��65   $��Center=�6B   $	��Top=�6_   
������� J� InputBindings 5p      d� 5q       d� Gesture$ 	LeftClicka�6#    �� Command 5r      �� *LogInCommand+5t      �� 5v      $��0��6   $��0��6%   $��3��65   $��2��6I   $��Center=�6Z   
�������$��60(�6�   $��	75 0 0 20q�6�   Регистрация6�   5w      5x      5y      