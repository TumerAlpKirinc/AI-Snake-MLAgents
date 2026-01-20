# 🐍 Unity ML-Agents: Snake AI

Bu proje, pekiştirmeli öğrenme (Reinforcement Learning) kullanarak kendi kendine oyun oynamayı öğrenen bir Yılan yapay zekasıdır.

## 🚀 Başarılar ve Deneyler 

| Model Versiyonu | Görüş Açısı / Sensör | Max Skor | Eğitim Adımı |
| :--- | :--- | :--- | :--- |
| **Ray Perception Olmayan** | Sensörsüz (Sadece Koordinat) | 81 | 7.000.000 |
| **Ray Perception Olan** | 90° Raycast (3 Işın) | 92 | 10.000.000 |

## 🧠 Teknik Özellikler
- **Algoritma:** PPO (Proximal Policy Optimization)
- **Gözlem:** Ray Perception Sensor 3D & Pozisyon verileri
- **Ödül Sistemi:** Yemek yeme (+5.0), Duvara/Kuyruğa çarpma (-1.0), Her adımda (-0.01) , Mesafe Ödülü (Manhattan Distance): Yılana yemeğe yaklaştığı her adım için (+0.01) uzaklaştığı her adım için (-0.01)
- **Curiosity:** Aktif (Keşif sürecini hızlandırmak için kullanıldı)

## 🗺️ Ortam Detayları (Environment)
- **Harita Boyutu:** 34x18 Grid

