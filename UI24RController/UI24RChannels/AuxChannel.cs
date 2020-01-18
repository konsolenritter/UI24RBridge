﻿using System;
using System.Collections.Generic;
using System.Text;
using UI24RController.UI24RChannels.Interfaces;

namespace UI24RController.UI24RChannels
{
    public class AuxChannel: ChannelBase, IStereoLinkable
    {

        public AuxChannel(int channelNumber): base(channelNumber)
        {
            this.Name = $"AUX {(this.ChannelNumber + 1):D2}";
            LinkedWith = -1; //-1: not linked, 0 left, 1 right
        }

        public override int ChannelNumberInMixer => this.ChannelNumber + 38;

        public int LinkedWith { get; set ; }

        public override string MixFaderMessage()
        {
            return $"3:::SETD^a.{this.ChannelNumber}.mix^{this.ChannelFaderValue.ToString().Replace(',', '.')}";
        }

        public override string MuteMessage()
        {
            return $"3:::SETD^a.{this.ChannelNumber}.mute^{(this.IsMute ? 1 : 0)}";
        }

        public override string SoloMessage()
        {
            return $"3:::SETD^a.{this.ChannelNumber}.solo^{(this.IsSolo ? 1 : 0)}";
        }
    }
}

