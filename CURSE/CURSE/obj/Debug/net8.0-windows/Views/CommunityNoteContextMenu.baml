   M S B A M L   `   `   `  ����O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  5   	   @ <CURSE, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:CURSE.ConverterCURSE.Converter 5   
   3clr-namespace:CURSE.ViewModelsCURSE.ViewModels 5	   	   *   $CURSE.Views.CommunityNoteContextMenu   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   6sys&clr-namespace:System;assembly=mscorlib     5   	   /localclr-namespace:CURSE.Converter     5   
   -vmclr-namespace:CURSE.ViewModels     5	   	   _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  5
   	     � Title$  CommunityNoteContextMenu��5   	   $	��500��5   	   $	��350��6    � WindowStyle$
 None=�5   	    � 
ResizeMode$ NoResize=�5   	    � WindowStartupLocation$ CenterOwner=�5   	    � AllowsTransparency$
 True��5   	   
������  � ShowInTaskbar$ False��5   	    � Topmost$
 True��5   	    � 	Resources 5      7  1CURSE.Converter.RichTextAndDateAndWindowConverter %  !RichTextAndDateAndWindowConverter��%   &	          5   
   62   5      .��5      � ���	#99000000.+�5   
   �� 
�������$��30(�6'   $��Center=�65   Текст заметки:6b   �� 5   
   $��Bottom=�6   $��	20 0 0 25q�60   $
��Left=�6C   .+�5      �� $ ��Выберите дату:��6   $��14(�5      $��0 0 0 5q�5      (  "System.Windows.Controls.DatePicker  5       -   5          Name$
 Date��5       ���#FFF
�������$��1q�5#      $��4q�5$      $	��160��5%      	 �� Today
   DisplayDateStart#
 Z	 5&      5(      �� 5)   
   $	��140��6   $��40��6   $��Right=�5*   	   $��Bottom=�5+   	   $��20(�5,   	   ��5-   	   �� *SaveAndLoadNotesCommand+��5.      `� 5/       `� 	Converter# [  6   .1�50      ��  �� ElementName$ RichTextBox��6   �� 51      $
 Date��6    �� Path$ SelectedDate�61   �� 52       �� RelativeSource 6   ��  �� AncestorType
 Window53      .��54   '   Опубликовать56      �� 58   
    �� CornerRadius$ 12��6   
������ $	��500��5:      $	��300��5;       �� Padding$ 20q�5<      $��Center=�5=      $	��top=�5>        
BlurEffect J� Effect# �  5?      ._�5A      �� -   5         ��Name$ RichTextBox��5A      $��0,30,0,0q�5B      $��18(�6#   
�������*  $Microsoft.Xaml.Behaviors.Interaction   	Behaviors 5C      -  'CURSE.ViewModels.RichTextBoxKeyBehavior  5D      9  3CURSE.ViewModels.DisableParentScrollOnFocusBehavior  5E      5G      5I      �� 5J   
   -   5        $��X��5J      $��30��5K      $��30��5L      $��Right=�5M      $	��Top=�5N      $��0 0 0 10q�5O      5P      5R      5T      