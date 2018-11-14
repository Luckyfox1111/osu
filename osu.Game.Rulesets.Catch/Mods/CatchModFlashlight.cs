﻿// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using osu.Framework.Graphics;
using osu.Game.Rulesets.Catch.Objects;
using osu.Game.Rulesets.Catch.UI;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.UI;
using OpenTK;

namespace osu.Game.Rulesets.Catch.Mods
{
    public class CatchModFlashlight : ModFlashlight<CatchHitObject>
    {
        public override double ScoreMultiplier => 1.12;

        private const float default_flashlight_size = 350;

        public override Flashlight CreateFlashlight() => new CatchFlashlight(playfield);

        private CatchPlayfield playfield;

        public override void ApplyToRulesetContainer(RulesetContainer<CatchHitObject> rulesetContainer)
        {
            playfield = (CatchPlayfield)rulesetContainer.Playfield;
            base.ApplyToRulesetContainer(rulesetContainer);
        }

        private class CatchFlashlight : Flashlight
        {
            private readonly CatchPlayfield playfield;

            public CatchFlashlight(CatchPlayfield playfield)
            {
                this.playfield = playfield;
                FlashlightSize = new Vector2(0, getSizeFor(0));
            }

            protected override void Update()
            {
                base.Update();

                var catcher = playfield.CatcherArea.MovableCatcher;

                FlashlightPosition = catcher.ToSpaceOfOtherDrawable(catcher.Position, this);
            }

            private float getSizeFor(int combo)
            {
                if (combo > 200)
                    return default_flashlight_size * 0.8f;
                else if (combo > 100)
                    return default_flashlight_size * 0.9f;
                else
                    return default_flashlight_size;
            }

            protected override void OnComboChange(int newCombo)
            {
                this.TransformTo(nameof(FlashlightSize), new Vector2(0, getSizeFor(newCombo)), FLASHLIGHT_FADE_DURATION);
            }

            protected override string FragmentShader => "CircularFlashlight";
        }
    }
}
