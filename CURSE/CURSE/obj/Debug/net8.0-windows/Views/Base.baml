   M S B A M L   `   `   `  ����@  <CURSE, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:CURSECURSE  5      )clr-namespace:CURSE.ViewsCURSE.Views  5      3clr-namespace:CURSE.ViewModelsCURSE.ViewModels  5      O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5          CURSE.Views.Base   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  5      localclr-namespace:CURSE  5      !vclr-namespace:CURSE.Views  5      'vmclr-namespace:CURSE.ViewModels  5      3System&clr-namespace:System;assembly=mscorlib  5        � Title$  
MainWindow��5	      $	��600��6    $	��800��6-    � WindowStyle$
 None=�5
       � WindowStartupLocation$ CenterScreen=�6     � 
ResizeMode$ CanResizeWithGrip=�5       � AllowsTransparency$
 True��6,   
������  � Icon$ /Views/Resources/Icon.png��5       � 	Resources 5   
    �� DataType.   (CURSE.ViewModels.CommunityNotesViewModel    CURSE.Views.CommunityNotes+   %CURSE.ViewModels.CanvasNotesViewModel   CURSE.Views.Notes��%�   (��       5      *, +)(�� 2     5      *, +)�� 5      "  6   .R�5        5      �� 5      "  6   .R�5        5      5      .��5   
   � ���#191919 � ShowGridLines$
 True��6$   	 � ColumnDefinitions	 5      �� 5      $	��200 �6#   $��50��6/   $	��350��6=   �� 5      $��5 �6#   �� 5      $��* �6#   
 � RowDefinitions
 5      �� 5       $
��Auto �6    �� 5"      $��* �6    .+�5&      �� ���#29222c$��1��6*   $��0��67   ���#29222c._�5'      ��  �� Orientation$ Vertical=�6   $	��Top=�64   .+�5(      �� $��	Community��6   $��10 15 10 10 q�5*      $��5q�5+      
����������#3A3A3A���#444$
��Bold"�5/      ��5)      �� *SwitchToCommunityCommand+5/      �� 50      
���  ��$��2��61   �� 52      
������ $��0q�54   	   $��0q�55   	   $��Center=�56   	    � Cursor$
 Hand��57   	   ��52      �� *AddCanvasCommand+.��58      �� $��/Views/Resources/Plus.png��6   $��30��6?   59      �� 5:      ��6   �� *
Canvases+��5;      �� 5<      .R�5=      �� $
��10,5q�5@      $��5q�5A      
����������#3A3A3A���#444$
��Bold"�5E      ��5=   %   �� *Title+��5>      �� *!DataContext.SelectCanvasCommand+ �� RelativeSource ��  �� AncestorType
 Window��5?      �� 5E      5F      5H      5I      5J      �� 5M      $��1��6   $��2��6+   $��Center=�5N      $��Stretch=�5O      
��� ���$��5��5P   /    �� ResizeBehavior$ PreviousAndNext=�5Q       �� ResizeDirection$ Columns=�5R      $ SizeWE��65   �� 5T      $��2��6:   $��2��6G   ��6   �� *CurrentViewModel+6G   �� 5Y      -   5        ���#29222c$��0��5Y   *   $��3��67   6K   �� 5Z      -   5        $��/Views/Resources/Icon.png��5Z      $
��Left=�5\      $��Center=�5]      $��25��5^      $��5,0,0,0q�5_      $��0��5`      �� 5a      -   5        
�������$��Center=�5c      $	��100��5e      $
��Left=�5f      $��20(�5g      $��32,0,0,0q�5h      $��0��5i      $��2��5j      ��5a      �� *
NickName+5j      �� 5k      $ 
Horizontal=�6   $��Right=�5l      $	��Top=�5m      $��0��5n      $��2��6&   .+�5o      �� -   5        ���#29222c$��-��5o   .   $��30(�6:   $��40��6R   
����������#29222c6p   �� 5p      -   5        ���#29222c$��X��5p   .   $��30(�6:   $��40��6H   
����������#29222c6p   5q      5r      5s      