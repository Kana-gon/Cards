using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AnimatedSprite : MonoBehaviour
{
    [SerializeField, Header("Relative path from StreamingAssets folder")] private string filePath;

    private SpriteRenderer _spriteRenderer;

    private readonly List<Sprite> _frames = new List<Sprite>();
    private readonly List<float> _frameDelay = new List<float>();

    private int _currentFrame = 0;
    private float _time = 0.0f;

    private void Start()
    {
        if (string.IsNullOrWhiteSpace(filePath)) return;
        _spriteRenderer = GetComponent<SpriteRenderer>();

        var path = Path.Combine(Application.streamingAssetsPath, filePath);

        using (var decoder = new MG.GIF.Decoder(File.ReadAllBytes(path)))
        {
            var img = decoder.NextImage();

            while (img != null)
            {
                _frames.Add(Texture2DtoSprite(img.CreateTexture()));
                _frameDelay.Add(img.Delay / 1000.0f);
                img = decoder.NextImage();
            }
        }

        if (_frames.Count > 0)
        {
            _spriteRenderer.sprite = _frames[0];
        }
    }

    private void Update()
    {
        if (_frames == null || _frames.Count == 0) return;

        _time += Time.deltaTime;

        if (_time >= _frameDelay[_currentFrame])
        {
            _currentFrame = (_currentFrame + 1) % _frames.Count;
            _time = 0.0f;

            _spriteRenderer.sprite = _frames[_currentFrame];
        }
    }

    private static Sprite Texture2DtoSprite(Texture2D tex)
    {
        // 黒残像を防ぐために、クリア済みの新しいテクスチャを作成
        Texture2D newTex = new Texture2D(tex.width, tex.height, TextureFormat.RGBA32, false);

        // 黒くなる要因を防ぐために、すべてのピクセルを透明に初期化
        Color[] clearPixels = new Color[tex.width * tex.height];
        for (int i = 0; i < clearPixels.Length; i++)
        {
            clearPixels[i] = Color.clear;
        }
        newTex.SetPixels(clearPixels);

        // 元の画像のピクセルをコピー
        newTex.SetPixels(tex.GetPixels());
        newTex.Apply();

        // フィルターモードを設定して画質劣化を防止
        newTex.filterMode = FilterMode.Point;
        newTex.wrapMode = TextureWrapMode.Clamp;

        return Sprite.Create(newTex, new Rect(0, 0, newTex.width, newTex.height), new Vector2(0.5f, 0.5f));
    }


}
