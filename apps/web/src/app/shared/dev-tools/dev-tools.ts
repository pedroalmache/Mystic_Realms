import { Component, signal, computed, OnDestroy, inject } from '@angular/core';

import { AudioService, AudioInstance } from '../../core/services/audio.service';
import { Assets } from '../../core/constants/assets.constants';

export type AssetCategory = 'audio' | 'images' | 'animations';

export interface AssetEntry {
  name: string;
  path: string;
}

export interface FolderNode {
  name: string;
  children: FolderNode[];
  files: AssetEntry[];
  totalCount: number;
}

function buildTree(obj: object, name: string): FolderNode {
  const node: FolderNode = { name, children: [], files: [], totalCount: 0 };
  for (const [key, value] of Object.entries(obj)) {
    if (typeof value === 'string') {
      node.files.push({ name: key, path: value });
    } else if (typeof value === 'object' && value !== null) {
      node.children.push(buildTree(value, key));
    }
  }
  node.totalCount = node.children.length + node.files.length;
  return node;
}

const ASSET_TREES: Record<AssetCategory, FolderNode> = {
  audio: buildTree(Assets.Audio, 'Audio'),
  images: buildTree(Assets.Images, 'Images'),
  animations: buildTree(Assets.Animations, 'Animations'),
};

@Component({
  selector: 'app-dev-tools',
  standalone: true,
  templateUrl: './dev-tools.html',
  styleUrl: './dev-tools.scss',
})
export class DevToolsComponent implements OnDestroy {
  private readonly audioService = inject(AudioService);

  protected readonly menuOpen = signal(false);
  protected readonly dialogCategory = signal<AssetCategory | null>(null);
  protected readonly copiedAsset = signal<string | null>(null);
  protected readonly folderPath = signal<FolderNode[]>([]);

  protected readonly currentFolder = computed(() => {
    const path = this.folderPath();
    return path.length > 0 ? path[path.length - 1] : null;
  });

  private readonly activeAudio = new Map<string, AudioInstance>();
  private copiedTimeout: ReturnType<typeof setTimeout> | null = null;

  protected toggleMenu(): void {
    this.menuOpen.update((v) => !v);
  }

  protected openDialog(category: AssetCategory): void {
    this.menuOpen.set(false);
    this.stopAllAudio();
    this.dialogCategory.set(category);
    this.folderPath.set([ASSET_TREES[category]]);
  }

  protected openFolder(folder: FolderNode): void {
    this.folderPath.update((path) => [...path, folder]);
  }

  protected navigateTo(index: number): void {
    this.folderPath.update((path) => path.slice(0, index + 1));
  }

  protected closeDialog(): void {
    this.stopAllAudio();
    this.dialogCategory.set(null);
    this.folderPath.set([]);
  }

  protected playAudio(asset: AssetEntry): void {
    this.stopAudio(asset);
    const instance = this.audioService.play(asset.path, false);
    this.activeAudio.set(asset.path, instance);
  }

  protected stopAudio(asset: AssetEntry): void {
    const instance = this.activeAudio.get(asset.path);
    if (instance) {
      instance.stop();
      this.activeAudio.delete(asset.path);
    }
  }

  protected copyToClipboard(asset: AssetEntry): void {
    navigator.clipboard.writeText(asset.path).catch((err) => {
      console.warn('DevTools: clipboard write failed', err);
    });
    if (this.copiedTimeout !== null) {
      clearTimeout(this.copiedTimeout);
    }
    this.copiedAsset.set(asset.path);
    this.copiedTimeout = setTimeout(() => {
      this.copiedAsset.set(null);
      this.copiedTimeout = null;
    }, 1500);
  }

  private stopAllAudio(): void {
    this.activeAudio.forEach((instance) => instance.stop());
    this.activeAudio.clear();
  }

  ngOnDestroy(): void {
    this.stopAllAudio();
    if (this.copiedTimeout !== null) {
      clearTimeout(this.copiedTimeout);
    }
  }
}
