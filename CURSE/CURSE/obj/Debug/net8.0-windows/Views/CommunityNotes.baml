   M S B A M L   `   `   `  ����@  <CURSE, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:CURSE.ViewModelsCURSE.ViewModels  5      1clr-namespace:CURSE.ConverterCURSE.Converter  5      3clr-namespace:CURSE.ViewModelsCURSE.ViewModels  5
           CURSE.Views.CommunityNotes   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  5      *localclr-namespace:CURSE.ViewModels  5      (convclr-namespace:CURSE.Converter  5      'vmclr-namespace:CURSE.ViewModels  5
        � 	Resources  5      8   2CURSE.Converter.NicknameMatchToVisibilityConverter   NicknameMatchConverter��%   &	          5   
   62   5      .��5      � .+�5   
   �� -   5         ��Name$ NotesScrollViewer��5      $
��Auto=�5      $��Disabled=�5      .��5      �� .+�5      � .+�5      �� $	��140��6   $��30��6*   $
��Left=�66   $	��Top=�6Q   $��	20,15,0,0q�6i   $��20(�6|   ��5      �� *
FilterText+ �� UpdateSourceTrigger$ PropertyChanged=��� 5      
�������$��40(�63   $��Center=�6A   	Community5      �� 5!      $%��Добавить заметку��6   $��20��5"      $��Right=�5#      $��10,15,20,10q�5$      ��5%      �� *AddCommunityNoteCommand+5&      �� 5)      ��6   �� *CommunityView+��5*      �� 5+      .R�5,      �� 5-      ��50      �� 51      .R�52      �� 
�������$��1q�53   %   
�������$��10q�55   %   $	��400��56   %   $	��150��57   %   ._�58   "   � .+�59   &   �� $��Hidden=�63   .��5:   *   �� $��15(�5;   4   $
��Wrap=�5<   4    �� Padding$ 0,30,0,0q�5=   4   $	��Top=�5>   4   ��5:   4   �� *Text+*  $Microsoft.Xaml.Behaviors.Interaction   	Behaviors 5?   .   9   3CURSE.ViewModels.DisableParentScrollOnHoverBehavior  5@   2   5B   +   5C   '   �� 5E   &   $	��400��60   $��30��6<   
���  ��$	��Top=�6Y   $��Center=�6q   $
��Bold"�5G   0   
�������$��20(�5H   C   ��5F   0   �� *
Nickname+5H   C   �� 5J   &   $��X��6-   $��30��5K   -   $��30��5L   -   $
��Bold"�5M   -   
�������
���  ��$��Right=�5P   -   $	��Top=�5Q   -   ��5R   -   �� *(&DataContext.DeleteCommunityNoteCommand+ �� RelativeSource ��  �� AncestorType ItemsControl��5S   -   �� y�5T   *   `� 5U   .    `� 	Converter# [  6;   .1�5V   2   ��  �� Path$ DataContext.Nickname�6:    6V   ��  ItemsControl�� 5W   2   $ Nickname�6:   5X   /   5Z   '   5[   #   5\      5]      5_      5`      5a      �� 5c   
   $��40��6   $��40��6   $	��Top=�6(   $
��Left=�6@   $��	30,90,0,0q�6[   $��30(�6n   ��5d      �� *ScrollToTopCommand+��5e      �� 	 �� ElementName$	 NotesScrollViewer��.��6L   ⬆6O   5f      5g      