   M S B A M L   `   `   `  ����	  CURSE    CURSE.Views.LogIn   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5        � 	Resources  5	         ValidationErrorTemplate � ToolTip��%�   &	        �� 5
   
   .R�5      ]� $
��True��6   .+�5      �� $	��⚠��6   
���  ��$��16(�5      $��2q�5       5      �� *[0].ErrorContent+�� 5      5      5      5      .��5      �  � ShowGridLines$ False��6   .+�5   
   � $��1��6    � ColumnDefinitions 5      �� 5      �� 5      �� 5       � RowDefinitions 5      �� 5      �� 5       �� 5!      �� 5"      �� 5#      �� 5$      �� 5%      �� 5&      �� 5'      .+�5*      �� ���#191919$��3��6*   $��10��6>   �� 5,      $��2��6   
�������$��50,12q�68   $��20(�6G   	E-mail:6^   �� 5.      $��2��6   $��0��6#   $��3��63   $��2��6G   $��30(�5/      $��Italic$�6$   $��80,40,80,20q�67    =� ErrorTemplate# [  50      ��51      �� *LoginVM.Email+ �� UpdateSourceTrigger$ PropertyChanged=� �� ValidatesOnDataErrors$
 True�� �� NotifyOnValidationError$
 True��.��52   7   ��53      �� 54      	 �� 
TargetType"	 ��6   
 �� Triggers
 55      P� 56       P� Property �@=�HasError P� Value$
 True��6E   . �57   "   ��  �� Property �@�ToolTip �� Value 58   )   ��  �� RelativeSource �� *Self+ �� Path$) #(Validation.Errors)[0].ErrorContent��� 59   "    � ��
 �  ��6@   �� 5:   "    � ��$ 2q�6D   5;      5=      .��5>   !   5?      �� 5A      $��3��6   
�������$��50,40q�68   $��2��6G   $��18(�6X   Пароль:6o   G� 5C      -   5        $��4��5C      $��0��6'   $��3��67   $��2��6K   $��30(�5D      $��Italic$�6   $��80,20,80,40q�6/   # [  5J      
 � Tag 5F      �� *LoginVM.Password+$ PropertyChanged=�$
 True��$
 True����5K      �� 5L      "	 G�6   
 5M      P� 5N       �@=�HasError$
 True��6E   . �5O   "   ��  �@�ToolTip 5P   )   ��  �� *Self+$) #(Validation.Errors)[0].ErrorContent��� 5Q   "    � ��
 �  ��6@   �� 5R   "    � ��$ 2q�6D   5S      5U      5W      �� 5Y      $��6��6   $��1��6"   $��Вход��5Z      ��5[      �� *EnterCommand+{�5\      �� *LoginVM.CanLogin+�� 5^      $��7��6   $��1��6%   $��Center=�65   $	��Top=�6R   
��������� 5_      $��регистрация��6   $��Center=�6/   $	��Top=�6L   
������� J� InputBindings 5`      d� 5a       d� Gesture$ 	LeftClicka�6'    �� Command 6;   �� *RegisterCommand+5c      5d      �� 5f      $��1��6   $��1��6%   $��2��65   $	��Top=�5g      
�������$��60(�5h      $��15 0 0 0q�6&   
Вход6>   5i      5j      5k      