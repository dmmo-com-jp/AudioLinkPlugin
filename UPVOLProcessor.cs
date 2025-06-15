using YukkuriMovieMaker.Player.Audio;
using YukkuriMovieMaker.Player.Audio.Effects;

namespace YMM4SamplePlugin.AudioEffect
{
    internal class UPVOLProcessor : AudioEffectProcessorBase
    {
        readonly UPVOL item;
        readonly TimeSpan duration;

        //出力サンプリングレート。リサンプリング処理をしない場合はInputのHzをそのまま返す。
        public override int Hz => Input?.Hz ?? 0;

        //出力するサンプル数
        public override long Duration => (long)(duration.TotalSeconds * Input?.Hz ?? 0) * 2;

        public UPVOLProcessor(UPVOL item, TimeSpan duration)
        {
            this.item = item;
            this.duration = duration;
        }

        //シーク処理
        protected override void seek(long position)
        {
            Input?.Seek(position);
        }

        //エフェクトを適用する
        protected override int read(float[] destBuffer, int offset, int count)
        {
            Input?.Read(destBuffer, offset, count);
            for (int i = 0; i < count; i += 2)
            {
                var volume = (float)(item.Volume.GetValue((Position + i) / 2, Duration / 2, Hz)+100)*(float)0.01;
                if ((string)VM.Get("" + (Position + i) * 0.5) =="true") {
                    destBuffer[offset + i + 0] *= volume;
                    destBuffer[offset + i + 1] *= volume;
                }
                
            }
            return count;
        }
    }
}