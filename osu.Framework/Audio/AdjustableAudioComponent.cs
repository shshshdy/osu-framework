﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Bindables;

namespace osu.Framework.Audio
{
    public class AdjustableAudioComponent : AudioComponent, IAudioAdjustment
    {
        private readonly HashSet<BindableDouble> volumeAdjustments = new HashSet<BindableDouble>();
        private readonly HashSet<BindableDouble> balanceAdjustments = new HashSet<BindableDouble>();
        private readonly HashSet<BindableDouble> frequencyAdjustments = new HashSet<BindableDouble>();

        /// <summary>
        /// Global volume of this component.
        /// </summary>
        public readonly BindableDouble Volume = new BindableDouble(1)
        {
            MinValue = 0,
            MaxValue = 1
        };

        protected readonly BindableDouble VolumeCalculated = new BindableDouble(1)
        {
            MinValue = 0,
            MaxValue = 1
        };

        /// <summary>
        /// Playback balance of this sample (-1 .. 1 where 0 is centered)
        /// </summary>
        public readonly BindableDouble Balance = new BindableDouble
        {
            MinValue = -1,
            MaxValue = 1
        };

        protected readonly BindableDouble BalanceCalculated = new BindableDouble
        {
            MinValue = -1,
            MaxValue = 1
        };

        /// <summary>
        /// Rate at which the component is played back (affects pitch). 1 is 100% playback speed, or default frequency.
        /// </summary>
        public readonly BindableDouble Frequency = new BindableDouble(1);

        protected readonly BindableDouble FrequencyCalculated = new BindableDouble(1);

        protected AdjustableAudioComponent()
        {
            Volume.ValueChanged += e => InvalidateState(e.NewValue);
            Balance.ValueChanged += e => InvalidateState(e.NewValue);
            Frequency.ValueChanged += e => InvalidateState(e.NewValue);
        }

        internal void InvalidateState(double newValue = 0) => EnqueueAction(OnStateChanged);

        internal virtual void OnStateChanged()
        {
            VolumeCalculated.Value = volumeAdjustments.Aggregate(Volume.Value, (current, adj) => current * adj.Value);
            BalanceCalculated.Value = balanceAdjustments.Aggregate(Balance.Value, (current, adj) => current + adj.Value);
            FrequencyCalculated.Value = frequencyAdjustments.Aggregate(Frequency.Value, (current, adj) => current * adj.Value);
        }

        public void AddAdjustmentDependency(AdjustableAudioComponent component)
        {
            AddAdjustment(AdjustableProperty.Balance, component.BalanceCalculated);
            AddAdjustment(AdjustableProperty.Frequency, component.FrequencyCalculated);
            AddAdjustment(AdjustableProperty.Volume, component.VolumeCalculated);
        }

        public void RemoveAdjustmentDependency(AdjustableAudioComponent component)
        {
            RemoveAdjustment(AdjustableProperty.Balance, component.BalanceCalculated);
            RemoveAdjustment(AdjustableProperty.Frequency, component.FrequencyCalculated);
            RemoveAdjustment(AdjustableProperty.Volume, component.VolumeCalculated);
        }

        public void AddAdjustment(AdjustableProperty type, BindableDouble adjustBindable) => EnqueueAction(() =>
        {
            switch (type)
            {
                case AdjustableProperty.Balance:
                    if (balanceAdjustments.Contains(adjustBindable))
                        throw new ArgumentException("An adjustable binding may only be registered once.");

                    balanceAdjustments.Add(adjustBindable);
                    break;
                case AdjustableProperty.Frequency:
                    if (frequencyAdjustments.Contains(adjustBindable))
                        throw new ArgumentException("An adjustable binding may only be registered once.");

                    frequencyAdjustments.Add(adjustBindable);
                    break;
                case AdjustableProperty.Volume:
                    if (volumeAdjustments.Contains(adjustBindable))
                        throw new ArgumentException("An adjustable binding may only be registered once.");

                    volumeAdjustments.Add(adjustBindable);
                    break;
            }

            InvalidateState();
        });

        public void RemoveAdjustment(AdjustableProperty type, BindableDouble adjustBindable) => EnqueueAction(() =>
        {
            switch (type)
            {
                case AdjustableProperty.Balance:
                    balanceAdjustments.Remove(adjustBindable);
                    break;
                case AdjustableProperty.Frequency:
                    frequencyAdjustments.Remove(adjustBindable);
                    break;
                case AdjustableProperty.Volume:
                    volumeAdjustments.Remove(adjustBindable);
                    break;
            }

            InvalidateState();
        });

        protected override void Dispose(bool disposing)
        {
            volumeAdjustments.Clear();
            balanceAdjustments.Clear();
            frequencyAdjustments.Clear();

            base.Dispose(disposing);
        }

        IBindable<double> IAudioAdjustment.Volume => Volume;
        IBindable<double> IAudioAdjustment.Balance => Balance;
        IBindable<double> IAudioAdjustment.Frequency => Frequency;
    }

    public enum AdjustableProperty
    {
        Volume,
        Balance,
        Frequency
    }
}
