   M S B A M L   `   `   `  ����@  <CURSE, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:CURSE.ViewsCURSE.Views  5      3clr-namespace:CURSE.ViewModelsCURSE.ViewModels  5          CURSE.Views.Notes   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      +localclr-namespace:CURSE.Views     5      -vmclr-namespace:CURSE.ViewModels     5      _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  5        � 	Resources  5   
      ResizeThumbTemplate �� 
TargetType � Cursor �� Stretch��%}  &	        �� 5      " n�6:   .R�5      � ���	#01000000$ SizeNWSE��6.   .+�5      F� "����    ��� р�� ��� р��     	
�������$��2��5   -   $
 None=�5      $��Right=�6*   $��Bottom=�5      F� 5      "����    ��� р�� ��� р��     	
�������$��1��5   *   $
 None=�5      $��Right=�6*   $��Bottom=�5      5      5      5      .��5   
   �  � RowDefinitions 5      �� 5      $��40 �6    �� 5      .+�5    &   �� $
��Left=�6.   $	��220��6I   $��30��6U   $��20(�6a   ��5!   .   �� *Title+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�6x   �� 5"       �� Orientation$ 
Horizontal=�6   $��Right=�62   $��0��6N   .+�5$      �� $��/Views/Resources/Plus.png��6   $
 Hand��6;   $��5q�6I   *  $Microsoft.Xaml.Behaviors.Interaction   Triggers 5%      +  %Microsoft.Xaml.Behaviors.EventTrigger  5&      	   	EventName$	 MouseLeftButtonDown��6)   *  $Microsoft.Xaml.Behaviors.TriggerBase
   Actions.
 5'      2  ,Microsoft.Xaml.Behaviors.InvokeCommandAction     Command 60   �� *AddSmallNoteCommand+5(      5*      �� 5+      $$��/Views/Resources/Underline.png��6   $
 Hand��6<   $��5q�6J    5,        5-      $	 MouseLeftButtonDown��6%   .
 5.         60   �� *ToggleUnderlineCommand+5/      51      �� 53      $!��/Views/Resources/Italic.png��6   $
 Hand��69   $��5q�6G    54        55      $	 MouseLeftButtonDown��6%   .
 56         60   �� *ToggleItalicCommand+57      59      �� 5;      $ 
Horizontal=�6   $��10,0,0,0q�6'   .+�5<      �� $ ��Размер шрифта:��6   
�������$��Center=�6H   $��0,0,5,0q�6c   �� 5=      
�������$��Center=�6R   $��0,0,5,0q�6m   ��6   �� *SelectedFontSize+6m   �� 5>      $��+��6   $��25��6%   $��25��60   $��0,0,5,0q�6<   ��6M   �� *AddFontSizeCommand+�� 5?      $��-��6   $��25��6%   $��25��60   ��6<   �� *RemoveFontSizeCommand+5@      �� 5B      $��/Views/Resources/Bold.png��6   $��5q�67   $
 Hand��5C       5D        5E      $	 MouseLeftButtonDown��6)   .
 5F         64   �� *ToggleBoldCommand+5G      5I      �� 5J      $��60��6   $��5q�6"   
�������$ �� IsSynchronizedWithCurrentItem$
 TrueY�5L       �� 
IsEditable$
 True��5M       �� StaysOpenOnEdit$
 True��5N       �� IsTextSearchEnabled$ False��5O      $
|�True��5P       �� SelectedItem 5K      �� *SelectedFontSize+$ TwoWay=�$ PropertyChanged=�  5R         ComboBoxToggleButtonTemplate � ColumnDefinitions F�Name��%�  &	       �� 5S      " d�6K   .R�5T      � 
������  5U      �� 5V   "   �� 5W   "   $��20 �63   .+�5[      �� $��0��6/   #��z��5\   !   $��Center=�5]   !   $
��Left=�5^   !   $	��3,0q�5_   !   F� 5a      $ Arrow��63   $��1��6#   
���   �$��Center=�5c      $��Center=�5d      "����        � 	=  	= � z     	5e      5f      5g      5h      ��5j      �� 5k      " ��6&   .R�5l      � .+�5m      d� $��ToggleButton��6+   #��[ 5n      $��Stretch=�5o      $��Stretch=�5p      $
|�True��5q       +� IsFocusScope$
 True��5t       d� 	IsChecked 5r      �� *IsDropDownOpen+$ TwoWay=� �� RelativeSource �� *TemplatedParent+��5w   "   �� 5x   &   ��60   �� *SelectedItem+ ��  �� AncestorType ComboBox5{      � 5}      $��Popup��6$   $��Bottom=�5       � 	StaysOpen$ False��5�      $
|�True��5�      ��5~      �� *IsDropDownOpen+ �� *TemplatedParent+.��5�   "   �� 
�������$��1q�5�      
������� J� SnapsToDevicePixels$
 True��5�      ._�5�   &   �� $	��200��63   .��5�   *   �� 5�   '   5�   #   5�      5�      5�         	Behaviors 5�      '   !CURSE.ViewModels.FontSizeBehavior  5�      ��5�      O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5�      �� 5�      0sys&clr-namespace:System;assembly=mscorlib  5�       �� Type" X�5�      .s�5�      X�8Q�6(   X�5�      10Q�6)   X�5�      12Q�6)   X�5�      14Q�6)   X�5�      16Q�6)   X�5�      18Q�6)   X�5�      24Q�6)   X�5�      36Q�6)   5�      5�      5�      �� 5�      -   5         ��Name$ MainScrollViewer��5�      $��2��5�      $��2��6    $
��Auto=�5�      $
��Auto=�5�      $��
10,0,10,10q�5�      .��5�      �� -   5         ��Name$
 Note��5�      
������ $
��auto��6@   $
��1500��6N   .+�5�      �� ��6#   �� *Notes+��5�      �� 5�      .R�5�   "   �� 5�      ��5�      �� 5�      .R�5�   "   �� .+�5�   "   � $	��200��6'   $	��180��5�      
������  �� Left 5�      �� *X+$ TwoWay=�
 �� Top 5�      �� *Y+$ TwoWay=� 5�   &   )   #CURSE.ViewModels.CanvasDragBehavior  5�   *   .+�5�   *   �� $��30��61   
���**��$	��Top=�6P   ._�5�   .   �� 
���  ��
���**��
���**��$��20(�6n   $��Right=�6|   $��20��6�   ��5�   7   �� *$"DataContext.DeleteSmallNoteCommand+ ��  UserControl��5�   7   �� .��5�   7   X6:   5�   +   �� 5�   &    ��Name$ MyRichTextBox��62   $��20(�5�   *     �� AcceptsReturn$
  True��5�   *   
��� ���$��0,30,0,0q�5�   *   ! �� IsDocumentEnabled$
! True��5�   *   (   "CURSE.ViewModels.RichTextBoxHelper"   BindableDocument" 5�   2   �� *	Content+$ TwoWay=�$ PropertyChanged=� 5�   *   )   #CURSE.ViewModels.PasteImageBehavior  5�   2   -	   'CURSE.ViewModels.RichTextBoxKeyBehavior	  5�   2   9
   3CURSE.ViewModels.DisableParentScrollOnFocusBehavior
  5�   .     5�   *   # �� 
TargetType$ �� Property% �� Value��%X   'J�       �� 5�   .   "# J�64   .�5�   2   �� $ �@��Margin$% 0q�6K   5�   /   5�   +   5�   '   n� 5�   &   & n�Name$& ResizeThumb��6,   -   5        $��15��5�   $   $��15��5�   $   $��Right=�5�   $   $��Bottom=�5�   $   $ SizeNWSE��5�   $   #��[  5�   $   ' K� ZIndex$' 10��5�   $   5�   #   5�   #   5�      5�      5�      5�      5�      5�      