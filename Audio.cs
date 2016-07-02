using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio;
using NAudio.Wave;
using NAudio.CoreAudioApi;
using Discord;
using Discord.Audio;
using Discord.Commands;
using Discord.Commands.Permissions.Levels;

namespace DiscordBot
{
    public class Audio
    {
        private static bool isPlaying = false;

        public static void StartMusic(string filePath, DiscordClient discordBot, Channel voiceChannel, IAudioClient aService)
        {
            if (!isPlaying)
            {
                var channelCount = discordBot.GetService<AudioService>().Config.Channels;
                var OutFormat = new WaveFormat(48000, 16, channelCount);
                using (var AudioReader = new AudioFileReader(filePath))
                using (var resampler = new MediaFoundationResampler(AudioReader, OutFormat))
                {
                    resampler.ResamplerQuality = 60; // Set the quality of the resampler to 60, the highest quality
                    int blockSize = OutFormat.AverageBytesPerSecond / 50;
                    byte[] buffer = new byte[blockSize];
                    int byteCount;
                    isPlaying = true;
                    while ((byteCount = resampler.Read(buffer, 0, blockSize)) > 0)
                    {
                        if (byteCount < blockSize)
                        {

                            for (int i = byteCount; i < blockSize; i++)
                                buffer[i] = 0;
                        }
                        aService.Send(buffer, 0, blockSize);
                    }
                    isPlaying = false;
                }
            }
        }

        public static void StopPlaying()
        {
            isPlaying = false;
        }
    }
}

