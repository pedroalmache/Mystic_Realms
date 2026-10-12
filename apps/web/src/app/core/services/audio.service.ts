import { Injectable } from '@angular/core';

export interface AudioInstance {
  stop(): void;
}

@Injectable({ providedIn: 'root' })
export class AudioService {
  play(src: string, isLoop: boolean, time?: number): AudioInstance {
    const audio = new Audio(src);
    audio.loop = isLoop;
    audio.play().catch(() => {});

    let timeoutId: ReturnType<typeof setTimeout> | null = null;

    if (time !== undefined) {
      timeoutId = setTimeout(() => {
        audio.pause();
        audio.currentTime = 0;
      }, time * 1000);
    }

    return {
      stop: () => {
        if (timeoutId !== null) {
          clearTimeout(timeoutId);
          timeoutId = null;
        }
        audio.pause();
        audio.currentTime = 0;
      },
    };
  }
}
