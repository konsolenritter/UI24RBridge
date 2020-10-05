﻿using System;
using System.Collections.Generic;
using System.Text;

namespace UI24RController.UI24RChannels

{
    public abstract class ChannelBase
    {
        /// <summary>
        /// Between 0 and 1.0
        /// </summary>
        public double ChannelFaderValue { get; set; }
        protected string _name = "";
        public virtual string Name {
            get
            {
                return _name;
            }
            set
            {
                if (value == "")
                {
                    _name = GetDefaultName();
                }
                else _name = value;
            }
        }

        public int ChannelNumber { get; internal set; }
        public bool IsSelected { get; set; }
        protected bool _muteBtn;
        public bool IsMute
        {
            get
            {
                return _muteBtn | ((_muteGroupMask & GlobalMuteGroup) > 0);
            }
            set
            {
                _muteBtn = value;
            }
        }
        protected UInt32 _muteGroupMask;
        protected UInt32 _muteGroupMaskDefault;
        public static UInt32 GlobalMuteGroup {get; set;}
        public UInt32 MuteGroupMask { 
            get
            {
                return _muteGroupMask;
            }
            set
            {
                _muteGroupMask = value | _muteGroupMaskDefault;
            }
        }
        public bool IsSolo { get; set; }
        public virtual int ChannelNumberInMixer => ChannelNumber;
        public Dictionary<SelectedLayoutEnum, double> AuxSendValues { get; set; }
        protected string channelTypeID { get; set; }

        public ChannelBase(int channelNumber)
        {
            ChannelFaderValue = 0;
            ChannelNumber = channelNumber;
            IsSelected = false;
            IsMute = false;
            _muteGroupMask = 0;
            _muteGroupMaskDefault = 1 << Mixer._muteAllBit;
            GlobalMuteGroup = 0;
            IsSolo = false;
            Name = GetDefaultName();
            AuxSendValues = new Dictionary<SelectedLayoutEnum, double>() 
            {
                {SelectedLayoutEnum.Aux1, 0 },
                {SelectedLayoutEnum.Aux2, 0 },
                {SelectedLayoutEnum.Aux3, 0 },
                {SelectedLayoutEnum.Aux4, 0 },
                {SelectedLayoutEnum.Aux5, 0 },
                {SelectedLayoutEnum.Aux6, 0 },
                {SelectedLayoutEnum.Aux7, 0 },
                {SelectedLayoutEnum.Aux8, 0 },
                {SelectedLayoutEnum.Fx1, 0 },
                {SelectedLayoutEnum.Fx2, 0 },
                {SelectedLayoutEnum.Fx3, 0 },
                {SelectedLayoutEnum.Fx4, 0 },
            };
            channelTypeID = "i";
        }

        protected virtual string GetDefaultName()
        {
            return "CH";
        }

        public virtual string SetAuxValueMessage(SelectedLayoutEnum selectedLayout)
        {
            int auxNumber = selectedLayout.AuxToInt();
            return $"3:::SETD^{this.channelTypeID}.{this.ChannelNumber}.aux.{auxNumber}.value^{this.AuxSendValues[selectedLayout].ToString().Replace(',', '.')}";
        }

        public virtual string SetFxValueMessage(SelectedLayoutEnum selectedLayout)
        {
            int auxNumber = selectedLayout.AuxToInt();
            return $"3:::SETD^{this.channelTypeID}.{this.ChannelNumber}.fx.{auxNumber}.value^{this.AuxSendValues[selectedLayout].ToString().Replace(',', '.')}";
        }

        public virtual string MixFaderMessage()
        {
            return $"3:::SETD^i.{this.ChannelNumber}.mix^{this.ChannelFaderValue.ToString().Replace(',', '.')}";
        }


        public virtual string SelectChannelMessage(string syncID)
        {
            return $"3:::BMSG^SYNC^{syncID}^{this.ChannelNumberInMixer}";
        }
        public virtual string MuteMessage()
        {
            return $"3:::SETD^i.{this.ChannelNumber}.mute^{(this.IsMute? 1: 0)}";
        }
        public virtual string SoloMessage()
        {
            return $"3:::SETD^i.{this.ChannelNumber}.solo^{(this.IsSolo ? 1 : 0)}";
        }
    }
}
