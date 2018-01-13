using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CheckersWPF.Enums;
using CheckersWPF.Properties;

namespace CheckersWPF.VMs
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        public SettingsViewModel()
        {
            var tmpTheme = Settings.Default.Theme;
            SelectedTheme = string.IsNullOrEmpty(tmpTheme) ? Theme.Wood : (Theme)Enum.Parse(typeof(Theme), tmpTheme);
            
            EnableSoundEffects = bool.Parse(Settings.Default.EnableSoundEffects);
            
            EnableMoveHints = bool.Parse(Settings.Default.EnableMoveHints);
        }

        private void AssignSetting(string name, string value)
        {
            Settings.Default.PropertyValues[name].PropertyValue = value;
            Settings.Default.Save();
        }

        public List<Theme> Themes =>
            Enum.GetValues(typeof(Theme)).Cast<Theme>().ToList();

        private Theme _selectedTheme;
        public Theme SelectedTheme
        {
            get { return _selectedTheme; }
            set
            {
                if (_selectedTheme != value)
                {
                    _selectedTheme = value;
                    OnPropertyChanged();
                }

                AssignSetting("Theme", value.ToString());
            }
        }

        private bool _enableSoundEffects;
        public bool EnableSoundEffects
        {
            get { return _enableSoundEffects; }
            set
            {
                if (_enableSoundEffects != value)
                {
                    _enableSoundEffects = value;
                    OnPropertyChanged();
                }

                AssignSetting("EnableSoundEffects", value.ToString());
            }
        }

        private bool _enableMoveHints;
        public bool EnableMoveHints
        {
            get { return _enableMoveHints; }
            set
            {
                if (_enableMoveHints != value)
                {
                    _enableMoveHints = value;
                    OnPropertyChanged();
                }

                AssignSetting("EnableMoveHints", value.ToString());
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}