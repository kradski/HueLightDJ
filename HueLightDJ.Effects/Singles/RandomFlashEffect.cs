using HueLightDJ.Effects.Base;
using Q42.HueApi.ColorConverters;
using Q42.HueApi.Streaming.Effects;
using Q42.HueApi.Streaming.Extensions;
using Q42.HueApi.Streaming.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HueLightDJ.Effects
{
  [HueEffect(Name = "Flashing Stars", IsBaseEffect = false, HasColorPicker = true, DefaultColor = "#FFFFFF")]
  public class RandomFlashEffect : IHueEffect
  {
    public Task Start(EntertainmentLayer layer, Func<TimeSpan> waitTime, RGBColor? color, CancellationToken cancellationToken)
    {
      //Non repeating effects should not run on baselayer
      if (layer.IsBaseLayer)
        return Task.CompletedTask;

      if (!color.HasValue)
        color = RGBColor.Random();

      Func<TimeSpan> customWaitMS = () => TimeSpan.FromMilliseconds((waitTime().TotalMilliseconds * 2) / layer.Count);


      var groupCount = layer.Count / 3;

      return layer.OrderBy(x => Guid.NewGuid()).ChunkByGroupNumber(groupCount).FlashQuick(cancellationToken, color, IteratorEffectMode.AllIndividual, IteratorEffectMode.RandomOrdered, waitTime: customWaitMS, duration: waitTime());
    }
  }
}